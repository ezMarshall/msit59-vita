using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using msit59_vita.Models;
using System;

namespace msit59_vita.Controllers
{
	//  前台 單一店家頁面 
	public class StoreController : Controller
	{
		private readonly VitaContext _context;
		public StoreController(VitaContext context)
		{
			_context = context;
		}

		//public IActionResult Index()
		//{
		//    return View();
		//}

		public IActionResult Index(int id)
		{
			var store = _context.Stores.First(x => x.StoreId == id);
			if (store == null)
			{
				return NotFound();
			}

			ViewData["Store"] = store;


			var products = _context.Products
							.Where(x => x.StoreId == id)
							.ToList();

			ViewData["Products"] = products;

			var productCategoryCounts = (from product in _context.Products
										 join category in _context.ProductCategories
										 on product.CategoryId equals category.CategoryId
										 where product.StoreId == id
										 group product by new { category.CategoryId, category.CategoryName } into g
										 select new
										 {
											 CategoryId = g.Key.CategoryId,
											 CategoryName = g.Key.CategoryName,
											 ProductCount = g.Count()
										 }).ToList();

			ViewBag.ProductCategoryCounts = productCategoryCounts;

			///
			//  取得店家的營業時間
			///
			// 獲取當前星期幾
			string shortDayOfWeekString = DateTime.Today.DayOfWeek.ToString().Substring(0, 3);

			//Console.WriteLine(shortDayOfWeekString);


			// 查詢對應的 StoreOpeningTime
			var storeOpeningTimeQuery = from row in _context.StoreOpeningHours
										where row.StoreId == id && row.MyWeekDay == shortDayOfWeekString
										select new
										{
											myWeekDay = row.MyWeekDay,
											storeOpenOrNot = row.StoreOpenOrNot,
											storeOpeningTime = row.StoreOpeningTime,
											storeClosingTime = row.StoreClosingTime,
										};
			var todayOpeningHours = storeOpeningTimeQuery.FirstOrDefault();
			ViewBag.storeOpeningTimeQuery = todayOpeningHours;

			var currentTime = DateTime.Now.TimeOfDay;


			var orderStatusTime = 0;
			if (todayOpeningHours.storeOpeningTime.HasValue && currentTime < todayOpeningHours.storeOpeningTime.Value.ToTimeSpan())
			{
				orderStatusTime = 2;
			}
			else if (todayOpeningHours.storeClosingTime.HasValue && currentTime > todayOpeningHours.storeClosingTime.Value.ToTimeSpan())
			{
				orderStatusTime = 3;
			}
			else
			{
				orderStatusTime = 4;
			}

			ViewBag.orderStatusTime = orderStatusTime;

			// 取得星星平均
			var averageReviewRating = from s in _context.Stores
								join o in _context.Orders on s.StoreId equals o.StoreId
								join r in _context.Reviews on o.OrderId equals r.OrderId
								where s.StoreId == id && r.ReviewRating != null
								select (decimal?)(r.ReviewRating);

			//var averageRating = averageReviewRating.Average() != null ? Math.Round(averageReviewRating.Average()??0 , 1) : 0;
			var averageRating = Math.Round(averageReviewRating.Average() ?? 0, 1);

			//Console.WriteLine(averageRating);
			ViewBag.averageRating = averageRating;

			// 取得評論數
			var reviewCount = from s in _context.Stores
							  join o in _context.Orders on s.StoreId equals o.StoreId
							  join r in _context.Reviews on o.OrderId equals r.OrderId
							  where s.StoreId == id
							  select r;

			var reviewCountSum = reviewCount.Count() != 0 ? reviewCount.Count().ToString()+ "評論" : "尚無評論";
			//Console.WriteLine(reviewCountSum);
			// 取得評論總和
			ViewBag.reviewCountSum = reviewCountSum;

			//使用者id目前寫固定值和登入寫true
			//判斷愛心數是否收藏
			ViewBag.isFavorite = false;
			if (true)
			{
				IsFavoriteStore favoriteStoreChecker = new IsFavoriteStore(_context);
				bool isFavorite = favoriteStoreChecker.FavoriteStore(1, id);
				ViewBag.isFavorite = isFavorite;
			}
			

			return View();
		}

		//登入驗證部分還未完成
		[HttpPost]
		public JsonResult ToggleFavorite(int storeId)
		{
			// 檢查用戶是否已登入
			//!User.Identity.IsAuthenticated
			if (false)
			{
				return Json(new { success = false, message = "請先登入" });

			}

			int customerId = 1;/* 獲取當前用戶的CustomerID */

			IsFavoriteStore favoriteStoreChecker = new IsFavoriteStore(_context);
			// 檢查當前用戶是否已收藏該商店
			bool isFavorite = favoriteStoreChecker.FavoriteStore(customerId, storeId);/* 您的邏輯來檢查該商店是否已被收藏 */;

            //Console.WriteLine(isFavorite);
            //bool isFavorite =true;
            // 切換收藏狀態
            if (isFavorite)
			{
				// 取消收藏
				/* 您的邏輯來取消收藏 */
				var favorite = _context.Favorites.FirstOrDefault(f => f.CustomerId == customerId && f.StoreId == storeId);
				if (favorite != null)
				{
					_context.Favorites.Remove(favorite);
				}
			}
			else
			{
				// 添加收藏
				/* 您的邏輯來添加收藏 */

				_context.Favorites.Add(new Favorite { CustomerId = customerId, StoreId = storeId });
				
			}
			_context.SaveChanges();

			return Json(new { success = true, isFavorite = !isFavorite });
		}

	}


}
