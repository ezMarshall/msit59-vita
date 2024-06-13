using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;

namespace msit59_vita.Controllers
{
    public class HistoricalOrdersController : Controller
    {

        private VitaContext _context;
        private int _customerId = 31;

        public HistoricalOrdersController(VitaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            //未登入 倒回首頁
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return Redirect("/");
            }


            //取得使用者ID
            var queryCustomer = from c in _context.Customers
                                where c.CustomerEmail == User.Identity.Name
                                select new
                                {
                                    c.CustomerId,
                                    c.CustomerName,
                                    c.CustomerNickName
                                };

            _customerId = queryCustomer.Single().CustomerId;


            var queryOrder = from o in _context.Orders
                             where o.CustomerOrderStatus > 2 && o.CustomerId == _customerId
                             orderby o.OrderTime descending
                             select new
                             {
                                 o.OrderId,
                                 o.Reviews,
                                 o.Store.StoreId,
                                 OrderPayment = o.OrderPayment ? "LINE PAY" : "現金",
                                 o.Store.StoreName,
                                 OrderAddress = o.OrderAddressCity + o.OrderAddressDistrict + o.OrderAddressDetails,
                                 o.Customer.CustomerName,
                                 o.OrderEinvoiceNumber,
                                 o.OrderPhoneNumber,
                                 OrderTime = o.OrderTime.ToString("yyyy/MM/dd HH:mm"),
                                 PredictedArrivalTime = o.PredictedArrivalTime.ToString("yyyy/MM/dd HH:mm"),
                                 OrderStoreMemo = String.IsNullOrEmpty(o.OrderStoreMemo) ? "無" : o.OrderStoreMemo,
                                 OrderUniformInvoiceVia = (int)o.OrderUniformInvoiceVia,
                                 OrderDeliveryVia = o.OrderDeliveryVia ? "外送" : "自取",
                                 CustomerOrderStatus = (int)o.CustomerOrderStatus,
                                 
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
            var queryReviews = from r in _context.Reviews
                               where r.Order.CustomerId == _customerId
                               select new
                               {
                                   r.ReviewId,
                                   r.OrderId
                               };
            ViewBag.reviews = queryReviews.ToList();
            ViewBag.customer = queryCustomer.Single();
            ViewBag.products = queryProducts.ToList();
            ViewBag.price = queryPrice.ToList();
            return View(queryOrder.ToList());
        }

        [HttpPost]
        public IActionResult Comment(int OrderId,string ReviewContent,byte ReviewRating)
        {


            //未登入 倒回首頁
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return Redirect("/");
            }


            Review review = new Review();
            review.OrderId = OrderId;
            review.ReviewContent = ReviewContent;
            review.ReviewRating = ReviewRating;
            review.ReviewTime = DateTime.Now;
            _context.Add(review);
            _context.SaveChanges();



            return Redirect("/HistoricalOrders");
        }
    }
}
