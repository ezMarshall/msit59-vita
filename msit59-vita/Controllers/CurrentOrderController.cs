using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;

namespace msit59_vita.Controllers
{
    public class CurrentOrderController : Controller
    {
        private VitaContext _context;
        private int _customerId = 31;

        public CurrentOrderController(VitaContext context)
        {
            _context = context;
        }
        /*
         * 根據訂單狀態取得當前訂單
         * 整理訂單內容
         * 總金額
         * 訂單狀態的變更
         * 取消訂單的顯示          
         * 
         */

        public IActionResult Canecl(int orderId) {


            //未登入 倒回首頁
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return Redirect("/");
            }

            Order order = _context.Orders.Find(orderId);
            order.CustomerOrderStatus = 5;
            _context.SaveChanges();

            return Redirect("/CurrentOrder");
        }

        public IActionResult Index()
        {


            //未登入 倒回首頁
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return Redirect("/");
            }

            //取得使用者ID
            var queryCustomerID = from c in _context.Customers
                                  where c.CustomerEmail == User.Identity.Name
                                  select c.CustomerId;
            _customerId = queryCustomerID.Single();


            var queryOrder = from o in _context.Orders
                             where o.CustomerOrderStatus < 3 && o.CustomerId == _customerId 
                             orderby o.OrderTime descending
                             select new
                             {
                                 o.OrderId,
                                 o.Store.StoreId,
                                 OrderPayment = o.OrderPayment ? "現金":"LINE PAY",
                                 o.Store.StoreName,
                                 o.Store.StoreImage,
                                 OrderAddress= o.OrderAddressCity + o.OrderAddressDistrict + o.OrderAddressDetails,
                                 o.Customer.CustomerName,
                                 o.OrderEinvoiceNumber,
                                 o.OrderPhoneNumber,
                                 OrderTime = o.OrderTime.ToString("yyyy/MM/dd HH:mm"),
                                 PredictedArrivalTime = o.PredictedArrivalTime.ToString("yyyy/MM/dd HH:mm"),
                                 OrderStoreMemo = String.IsNullOrEmpty(o.OrderStoreMemo) ?"無": o.OrderStoreMemo,
                                 OrderUniformInvoiceVia = (int)o.OrderUniformInvoiceVia,
                                 OrderDeliveryVia = o.OrderDeliveryVia ? "外送": "自取",
                                 CustomerOrderStatus = (int)o.CustomerOrderStatus
                             };

            var queryProducts = from o in _context.Orders
                                join od in _context.OrderDetails on o.OrderId equals od.OrderId
                                where o.CustomerOrderStatus >= 0 && o.CustomerId == _customerId
                                select new
                                {
                                    o.OrderId,
                                    od.Product.ProductName,
                                    od.UnitPrice,
                                    od.Quantity,
                                    o.OrderTime

                                };
            var queryPrice = from p in queryProducts
                             orderby p.OrderTime descending
                             group p by p.OrderId into g
                             select new
                             {
                                 OrderId = g.Key,
                                 totalPrice = (int)g.Sum(p => (p.UnitPrice * p.Quantity))
                             };

            ViewBag.products = queryProducts.ToList();
            ViewBag.price = queryPrice.ToList();
            return View(queryOrder.ToList());
        }
    }
}
