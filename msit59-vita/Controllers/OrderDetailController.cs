using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
		public IActionResult PayMentOrderDetail(string flexRadioPickUp, string flexRadioReceipt, string flexRadioPay, string flexRadioPhone, string carrier, string address, string phoneNumber, string orderTime, string orderNote)
		{


			//取餐時間
			HttpContext.Session.SetString("orderTime", orderTime);

			var inputTime = HttpContext.Session.GetString("orderTime");

			ViewBag.OrderTime = inputTime;



			//取餐方式

			HttpContext.Session.SetString("pickupMethod", flexRadioPickUp);

			var pickup = HttpContext.Session.GetString("pickupMethod");

			if (pickup == "0")
			{
				//到店取貨

				ViewBag.PickupMethod = "自取";

			}
			else if (pickup == "1")
			{
				ViewBag.PickupMethod = "外送";

				//外送要顯示取餐地址
				HttpContext.Session.SetString("address", address);

				var inputAddress = HttpContext.Session.GetString("address");

				ViewBag.Address = inputAddress;

			}

			//付款方式
			HttpContext.Session.SetString("PayMethod", flexRadioPay);

			var payMethod = HttpContext.Session.GetString("PayMethod");


			if (payMethod == "0")
			{
				//現金付款

				ViewBag.PayMethod = "現金";

			}
			else if (payMethod == "1")
			{
				//LINEPAY付款
				ViewBag.PayMethod = "LINEPAY";
			}



			//開立發票方式
			HttpContext.Session.SetString("invoiceMethod", flexRadioReceipt);

			var invoice = HttpContext.Session.GetString("invoiceMethod");

			//Console.WriteLine($"invoice: {invoice}");

			if (invoice == "0")
			{
				// 不開立發票
				ViewBag.InvoiceMethod = "本店免用統一發票";
			}
			else if (invoice == "1")
			{
				//紙本發票
				ViewBag.InvoiceMethod = "紙本發票";
			}
			else if (invoice == "2")
			{
				//手機載具
				ViewBag.InvoiceMethod = "手機載具";
				HttpContext.Session.SetString("Carrier", carrier);

				var inputCarrier = HttpContext.Session.GetString("Carrier");

				ViewBag.Carrier = inputCarrier;

			}


			//聯絡電話

			HttpContext.Session.SetString("phoneMethod", flexRadioPhone);

			var phone = HttpContext.Session.GetString("phoneMethod");

			//Console.WriteLine($"phone: {phone}");

			HttpContext.Session.SetString("phoneNumber", phoneNumber);

			var phoneNum = HttpContext.Session.GetString("phoneNumber");


			if (phone == "0")
			{
				// 手機
				ViewBag.phoneNum = phoneNumber;
			}
			else if (phone == "1")
			{
				// 市話
				ViewBag.phoneNum = phoneNumber;
			}


			//備註

			if (orderNote == null)
			{
				ViewBag.OrderNote = "無";
			}
			else
			{
				HttpContext.Session.SetString("orderNote", orderNote);
				var inputNote = HttpContext.Session.GetString("orderNote");
				ViewBag.OrderNote = inputNote;
			}






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

			var totalAmount = (int)paymentInfo.Sum(x => x.ProductUnitPrice * x.ShoppingCartQuantity);

			ViewBag.TotalAmount = totalAmount;



			//return Content($"flexRadioReceipt: {flexRadioReceipt}");
			return View();
		}
	}
}
