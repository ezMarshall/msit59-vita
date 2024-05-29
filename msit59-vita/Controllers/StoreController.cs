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

			var products = _context.Products
							.Where(x => x.StoreId == id)
							.ToList();

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

			//var productCategoryCounts = (from product in _context.Products
			//							 join category in _context.ProductCategories
			//							 on product.CategoryId equals category.CategoryId
			//							 where product.StoreId == id
			//							 group product by new { category.CategoryId, category.CategoryName } into g
			//							 select new 
			//							 {
			//								 CategoryId = g.Key.CategoryId,
			//								 CategoryName = g.Key.CategoryName,											 
			//							 }).ToList();
			//foreach (var item in productCategoryCounts)
			//{
			//	Console.WriteLine($"Category Id: {item.CategoryId}, Category Name: {item.CategoryName}, Product Count: {item.ProductCount}");
			//}


			//ViewData["ProductCategoryCounts"] = productCategoryCounts;
			//// 獲取當前星期幾
			//string shortDayOfWeekString = DateTime.Today.DayOfWeek.ToString().Substring(0, 3);

			////Console.WriteLine(shortDayOfWeekString);


			//// 查詢對應的 StoreOpeningTime
			//var storeOpeningTimeQuery = from row in _context.StoreOpeningHours
			//							where row.StoreId == id
			//							select new
			//							{
			//								myWeekDay = row.MyWeekDay,
			//								storeOpenOrNot = row.StoreOpenOrNot,
			//								storeOpeningTime = row.StoreOpeningTime,
			//								storeClosingTime = row.StoreClosingTime,
			//							};


			ViewData["Store"] = store;
			ViewData["Products"] = products;

			return View();
		}
	}

}
