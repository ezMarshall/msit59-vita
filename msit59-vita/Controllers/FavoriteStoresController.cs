using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using msit59_vita.Models;


namespace msit59_vita.Controllers
{



    public class FavoriteStoresController : Controller
    {

        private VitaContext _context;

        public FavoriteStoresController(VitaContext context)
        {
            _context = context;
        }

        
        /*
         * 取得使用者id CustomerID
         * 經由id查找資料 店家資料
         * 營業時間
         * 平均星級+評論數
         */

        public IActionResult Index( )            
        {
            ViewBag.CustomerID = 1;
            var query = from o in _context.Favorites
                        join s in _context.Stores on o.StoreId equals s.StoreId
                        where o.CustomerId == 1
                        select new
                        {
                            o.FavoriteId,
                            s.StoreId,
                            s.StoreName,
                            StoreAddress = s.StoreAddressCity + s.StoreAddressDistrict + s.StoreAddressDetails,
                            s.StorePhoneNumber,
                            s.StoreImage
                        };
            // todo 如何做left join?
            var queryOpeningHours = from o in _context.Favorites
                                    join s in _context.Stores on o.StoreId equals s.StoreId
                                    join so in _context.StoreOpeningHours on o.StoreId equals so.StoreId
                                    select new
                                    {                                        
                                        so.Store.StoreId,
                                        so.MyWeekDay,
                                        so.StoreOpenOrNot,
                                        StoreOpeningTime = so.StoreOpeningTime.ToString().Substring(0,5).Replace(":",""),
                                        StoreClosingTime = so.StoreClosingTime.ToString().Substring(0,5).Replace(":",""),
                                    };

            var queryComments = from f in _context.Favorites
                                join o in _context.Orders on f.StoreId equals o.StoreId
                                join r in _context.Reviews on o.OrderId equals r.OrderId
                                group r by r.Order.StoreId into g
                                select new
                                {
                                    StoreId = g.Key,
                                    avgOfRating = g.Average(re => re.ReviewRating),
                                    countOfRating= g.Count(re => re.ReviewId != 0 ).ToString()
                                };
            
            ViewBag.queryComments = queryComments.ToList();
            //queryOpeningHours.ToList().FindAll(i => i.StoreId == 1);
            ViewBag.openHours = queryOpeningHours.ToList();
            var obj = query.ToList();
            return View(obj);
        }



        public IActionResult Cancel(int id)
        {
            var  favoriteItem = _context.Favorites.Find(id);
            _context.Favorites.Remove(favoriteItem);
            _context.SaveChanges();

            //return View();
            return Redirect("/FavoriteStores");
            return RedirectToAction("Index", "FavoriteStores");
        }
    }
}
