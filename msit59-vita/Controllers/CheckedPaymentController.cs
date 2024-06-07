using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;
using System.Linq;

namespace msit59_vita.Controllers
{
	public class CheckedPaymentController : Controller
	{
		//  前台 確認付款頁面

		private readonly VitaContext _context;

		public CheckedPaymentController(VitaContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult Payment()
		{
            var customer = from c in _context.Customers
                           where c.CustomerEmail == User.Identity.Name
                           select c;

            var customerId = customer.FirstOrDefault();

            var customerID = customerId.CustomerId;/* 獲取當前用戶的CustomerID */

			ViewBag.CustomerName = customerId.CustomerName;



            // 獲取當前星期幾
            string shortDayOfWeekString = DateTime.Today.DayOfWeek.ToString().Substring(0, 3);

            //select*
            //from ShoppingCarts sc
            //join Customers ct on sc.CustomerID = ct.CustomerID
            //join Products p on sc.ProductID = p.ProductID
            //join Stores st on p.StoreID = st.StoreID
            //join StoreOpeningHours op on st.StoreID = op.StoreID
            //where sc.CustomerID = 2 and op.MyWeekDay = 'Wed'


            var cart = from c in _context.ShoppingCarts
					   join ct in _context.Customers on c.CustomerId equals ct.CustomerId
					   join p in _context.Products on c.ProductId equals p.ProductId
					   join s in _context.Stores on p.StoreId equals s.StoreId
					   join o in _context.StoreOpeningHours on s.StoreId equals o.StoreId
					   where c.CustomerId == customerID && o.MyWeekDay == shortDayOfWeekString
                       select new 
					   {
						   ShoppingCartId = c.ShoppingCartId,
						   ShoppingCartQuantity = c.ShoppingCartQuantity,
						   StoreId = s.StoreId,
						   StoreName = s.StoreName,
						   StoreLinePay = s.StoreLinePay,
						   StorePhoneNumber = s.StorePhoneNumber,
						   StoreUniformInvoiceVia = s.StoreUniformInvoiceVia,
						   CustomerName = ct.CustomerName,
						   ProductId = p.ProductId,
						   ProductName = p.ProductName,
						   ProductUnitPrice = p.ProductUnitPrice,
						   ProductUnitsInStock = p.ProductUnitsInStock,
					   };

			var paymentInfo = cart.ToList();

            ViewBag.PaymentInfo = paymentInfo;

            // 這裡是支付確認頁面的邏輯
            return View();
		}

		[HttpPost]
		public IActionResult PaymentConfirm()
		{
			// 先判斷使用者是否有登入，若無登入則導向登入頁面
			if (!User.Identity?.IsAuthenticated ?? false)
			{
				return Json(new { success = false, message = "請先登入" });
			}

			// 取得目前的訂單資料，透過購物車資料表查詢

			return Json(new { success = true, message = "Payment Success" });


		}

		[HttpPost]
		public IActionResult GetStoreOpenTime(int storeId)
		{
			// 獲取當前星期幾
			string shortDayOfWeekString = DateTime.Today.DayOfWeek.ToString().Substring(0, 3);

			// 查詢對應的 StoreOpeningTime
			var storeOpeningTimeQuery = from s in _context.StoreOpeningHours
										where s.StoreId == storeId && s.MyWeekDay == shortDayOfWeekString
										select new
										{
											myWeekDay = s.MyWeekDay,
											storeOpenOrNot = s.StoreOpenOrNot,
											storeOpeningTime = s.StoreOpeningTime,
											storeClosingTime = s.StoreClosingTime,
										};
            
			var storeOpeningTime = storeOpeningTimeQuery.FirstOrDefault();

			if (storeOpeningTime == null)
			{
				return Json(new { success = false, message = "Get Store Open Time Failed" });
			}

            return Json(new { success = true, message="Get Store Open Time Success", data = storeOpeningTime });
		}


		//帶入會員資料
		[HttpGet]
		public IActionResult GetMemberProfile()
		{

			var customer = _context.Customers
			   .FirstOrDefault(c => c.CustomerEmail == User.Identity.Name);

			if (customer == null)
			{
				return Json(new { success = false, message = "Customer not found" });
			}

			var customerProfile = new
			{
				address = $"{customer.CustomerAddressCity.Trim()}{customer.CustomerAddressDistrict.Trim()} {customer.CustomerAddressDetails.Trim()}",
				carrier = string.IsNullOrWhiteSpace(customer.CustomerEinvoiceNumber) ? null : customer.CustomerEinvoiceNumber.Trim(),
				phone = !string.IsNullOrEmpty(customer.CustomerLocalPhone) ? customer.CustomerLocalPhone.Trim() : null,
				cellphone = !string.IsNullOrEmpty(customer.CustomerCellPhone) ? customer.CustomerCellPhone.Trim() : null
			};

			return Json(new { success = true, data = customerProfile });

		}

		// 繼續加購功能
		[HttpGet]
		public IActionResult GetShopping()
		{
			var customer = from c in _context.Customers
						   where c.CustomerEmail == User.Identity.Name
						   select c;

			var customerId = customer.FirstOrDefault();
			var customerID = customerId.CustomerId;/* 獲取當前用戶的CustomerID */

			//select*
			//from Customers c join ShoppingCarts sc on c.CustomerID = sc.CustomerID
			//join Products p on p.ProductID = sc.ProductID
			//join Stores s on s.StoreID = p.StoreID
			//where c.CustomerID = 4

			var cart = from c in _context.ShoppingCarts
					   join ct in _context.Customers on c.CustomerId equals ct.CustomerId
					   join p in _context.Products on c.ProductId equals p.ProductId
					   join s in _context.Stores on p.StoreId equals s.StoreId
					   where c.CustomerId == customerID
                       select new
					   {
						   ShoppingCartId = c.ShoppingCartId,
						   ShoppingCartQuantity = c.ShoppingCartQuantity,
						   StoreId = s.StoreId,
					   };

			var shoppingInfo = cart.ToList();
						   



			return Json(new { success = true, data = shoppingInfo });
		}


	}
}
