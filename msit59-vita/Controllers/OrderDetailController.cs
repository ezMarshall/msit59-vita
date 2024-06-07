using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;

namespace msit59_vita.Controllers
{
    //  後台訂單詳細資訊頁面
    public class OrderDetailController : Controller
    {
		//public IActionResult Index()
		//{
		//    return View();
		//}

		private readonly VitaContext _context;

		public OrderDetailController(VitaContext context)
		{
			_context = context;
		}

		


		[HttpPost]
		public IActionResult PayMentOrderDetail(int flexRadioReceipt)
		{
			//訂購門市
			//取餐人
			//取餐方式
			//聯絡電話
			//----------------------

			//取餐方式
			//取餐時間
			//外送要顯示取餐地址
			//付款方式
			//開立發票方式

			//----------------------
			//訂單商品明細
			//訂單總金額


			var customer = from c in _context.Customers
						   where c.CustomerEmail == User.Identity.Name
						   select c;

			var customerId = customer.FirstOrDefault();

			var customerID = customerId.CustomerId;

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

			//return Content($"flexRadioReceipt: {flexRadioReceipt}");
			return View();
		}
	}
}
