using Microsoft.AspNetCore.Mvc;
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

			return View();
		}
	}
}
