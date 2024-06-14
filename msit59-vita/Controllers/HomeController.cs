using GeoCoordinatePortable;
using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;
using msit59_vita.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace msit59_vita.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private VitaContext _context;
        private List<StoreSearchViewModel>  _nowStoreList;

        public HomeController(ILogger<HomeController> logger, VitaContext context)
        {
            _logger = logger;
            _context = context;
            _nowStoreList = StoreList.NowStoreList ?? new List<StoreSearchViewModel>();
        }

        public IActionResult Index(string state = null)
        {
            if (!string.IsNullOrEmpty(state))
            {
                state = System.Web.HttpUtility.UrlDecode(state);

                // 將解碼後的 state 字符串反序列化為對象
                var searchState = JsonConvert.DeserializeObject<SearchStateViewModel>(state);

                // 獲取經緯度數據
                var data = (JObject)searchState.Data;
                double lat = (double)data["lat"];
                double lng = (double)data["lng"];
                GetStorList(lat, lng);
                StoreList.NowStoreList = _nowStoreList;
                ViewBag.isSearched = true;
                return View(_nowStoreList);
            }

            // 判斷是否登入
            if (User.Identity?.IsAuthenticated ?? false)
            {
                string myEmail = User.Identity?.Name ?? "";
                var Customer = _context.Customers
                    .Where(c => c.CustomerEmail == myEmail)
                    .FirstOrDefault();
                if (Customer != null) { 
                    ViewBag.Address = (Customer.CustomerAddressCity ?? "") + (Customer.CustomerAddressDistrict ?? "") + (Customer.CustomerAddressDetails ?? "");
                }

                // 是否有存常用地址
                if (Customer != null && Customer.CustomerAddressMemo != null)
                {
                    // 有的話根據常用地址搜尋
                    string[] Memo = Customer.CustomerAddressMemo.Split(',');
                    double result1, result2, lat, lng;
                    if (double.TryParse(Memo[0], out result1) && double.TryParse(Memo[1], out result2))
                    {
                        lat = result1;
                        lng = result2;
                        GetStorList(lat, lng);
                        StoreList.NowStoreList = _nowStoreList;
                        ViewBag.isSearched = true;
                    }
                }
            }
            return View(_nowStoreList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region 收藏店家
        [HttpPost]
        public IActionResult ToggleFavorite(int StoreId)
        {
            if (!(User.Identity?.IsAuthenticated ?? false))
            {
                return Json(new { success = false });
            }
            int CustomerId = _context.Customers.Where(c => c.CustomerEmail == User.Identity.Name).FirstOrDefault()!.CustomerId;
            IsFavoriteStore FavoriteChecker = new IsFavoriteStore(_context);
            bool isFavorite = FavoriteChecker.FavoriteStore(CustomerId, StoreId);
            if (isFavorite)
            {
                var favorite = _context.Favorites.FirstOrDefault(f => f.CustomerId == CustomerId && f.StoreId == StoreId);
                if (favorite != null)
                {
                    _context.Favorites.Remove(favorite);
                }
            }
            else
            {
                _context.Favorites.Add(new Favorite { CustomerId = CustomerId, StoreId = StoreId });
            }
            _context.SaveChanges();
            return Json(new { success = true });
        }
        #endregion

        #region 地點搜尋
        [HttpPost]
        public IActionResult StoreSearch(double lat, double lng)
        {
            GetStorList(lat, lng);
            StoreList.NowStoreList = _nowStoreList;
            ViewBag.isSearched = true;
            return PartialView("_StoreListPartial", _nowStoreList);
        }
        #endregion

        #region 精選推薦
        [HttpPost]
        public IActionResult SortRating()
        {
            _nowStoreList.Sort((x, y) => y.averageRating.CompareTo(x.averageRating));
            ViewBag.isSearched = true;
            return PartialView("_StoreListPartial", _nowStoreList);
        }
        #endregion

        #region 餐點搜尋
        [HttpPost]
        public IActionResult MealSearch(string search)
        {
            var productNamesWithSearch = _context.Products
                .Where(p => p.ProductName.Contains(search) 
                || _context.Stores.Any(s => s.StoreId == p.StoreId 
                && (s.StoreName.Contains(search) || s.StoreAddressDistrict.Contains(search) || s.StoreAddressDetails.Contains(search) )))
                .Select(p => p.StoreId)
                .Distinct()
                .ToList();

            _nowStoreList = _nowStoreList
            .Where(store => productNamesWithSearch.Contains(store.StoreId))
            .ToList();

            ViewBag.isSearched = true;
            return PartialView("_StoreListPartial", _nowStoreList);
        }
        #endregion

        #region 刪除搜尋
        [HttpPost]
        public IActionResult DeleteSearch()
        {
            ViewBag.isSearched = true;
            return PartialView("_StoreListPartial", _nowStoreList);
        }
        #endregion

        private void GetStorList(double lat, double lng)
        {
            var coord = new GeoCoordinate(lat, lng);

            _nowStoreList.Clear();
            var list = _context.Stores.ToList();
            foreach (var item in list)
            {
                string weekDate = DateTime.Today.DayOfWeek.ToString().Substring(0, 3);
                var openingQuery = _context.StoreOpeningHours.Where(o => o.StoreId == item.StoreId && o.MyWeekDay == weekDate).FirstOrDefault();
                var currentTime = DateTime.Now.TimeOfDay;

                // 距離五公里以內
                string[] Memo = item.StoreAddressMemo.Split(",");
                double result1, result2, storelat, storelng;
                double Dis = 99999;
                if (double.TryParse(Memo[0], out result1) && double.TryParse(Memo[1], out result2))
                {
                    storelat = result1;
                    storelng = result2;
                    Dis = (int)new GeoCoordinate(storelat, storelng).GetDistanceTo(coord);
                }

                // 今日無營業||已經關了||距離超過五公里
                bool StoreOpenToday = openingQuery.StoreOpenOrNot ?? false;
                bool StoreClosing = (openingQuery.StoreClosingTime.HasValue && currentTime > openingQuery.StoreClosingTime.Value.ToTimeSpan());
                if (!StoreOpenToday || StoreClosing || Dis > 5000)
                {
                    continue;
                }

                // 星級
                var averageReviewRating = from s in _context.Stores
                                          join o in _context.Orders on s.StoreId equals o.StoreId
                                          join r in _context.Reviews on o.OrderId equals r.OrderId
                                          where s.StoreId == item.StoreId && r.ReviewRating != null
                                          select (decimal?)(r.ReviewRating);
                Decimal averageRating = Math.Round(averageReviewRating.Average() ?? 0, 1);
                // 評論數
                var reviewCount = from s in _context.Stores
                                  join o in _context.Orders on s.StoreId equals o.StoreId
                                  join r in _context.Reviews on o.OrderId equals r.OrderId
                                  where s.StoreId == item.StoreId
                                  select r;
                int totalReviews = reviewCount.Count();
                // 收藏
                bool isFavorite = false;
                if (User.Identity?.IsAuthenticated ?? false)
                {
                    int customerId = _context.Customers.Where(c => c.CustomerEmail == User.Identity.Name).FirstOrDefault()!.CustomerId;
                    IsFavoriteStore favoriteStoreChecker = new IsFavoriteStore(_context);
                    isFavorite = favoriteStoreChecker.FavoriteStore(customerId, item.StoreId);
                }

                // 商品圖
                List<string> ProductImageList = new List<string>();
                var Products = _context.Products.Where(p => p.ProductImage != "").ToList();
                var RandomProducts = Products.OrderBy(x => Guid.NewGuid()).Take(4).ToList();
                for (int i = 0; i < 4; i++)
                {
                    ProductImageList.Add(RandomProducts[i].ProductImage ?? "image/Common/300x300_default.png");
                }

                var store = new StoreSearchViewModel
                {
                    StoreId = item.StoreId,
                    StoreName = item.StoreName,
                    StoreAddress = item.StoreAddressCity + item.StoreAddressDistrict + item.StoreAddressDetails,
                    StoreImage = (item.StoreImage != "" ? item.StoreImage : "image/Common/300x300_default.png"),
                    StoreOpeningTime = $"{openingQuery.StoreOpeningTime.Value.ToString("HH:mm")}-{openingQuery.StoreClosingTime.Value.ToString("HH:mm")}",
                    StoreOpening = (openingQuery.StoreOpeningTime.HasValue && currentTime > openingQuery.StoreOpeningTime.Value.ToTimeSpan()),
                    averageRating = averageRating,
                    totalReviews = totalReviews,
                    isFavorite = isFavorite,
                    ProductImageList = ProductImageList,
                };
                _nowStoreList.Add(store);
            }
        }
    }
}
