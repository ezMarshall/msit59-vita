using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;
using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;
using System.Drawing;

namespace msit59_vita.Controllers
{
    public class ManagerCommentsController : Controller
    {
        private readonly VitaContext _context;

        public ManagerCommentsController(VitaContext context)
        {
            _context = context;
        }

        public IActionResult ManagerComments(String searchString)
        {
            // 透過訂單ID 找到 顧客ID
            // 透過顧客ID 找到評論發布人
            // 透過 訂單ID 找到 訂單內容
            // 僅選出店家ID==1的店家
            // 以時間先後排序
            // group by reviewID 避免不同訂購項目分行顯示 (除了訂購品項外 從每個分組中選出第一筆資料)
            // 將訂購品項用頓號隔開
            var queryReviews = (from c in _context.Customers
                                join o in _context.Orders on c.CustomerId equals o.CustomerId
                                join r in _context.Reviews on o.OrderId equals r.OrderId
                                join od in _context.OrderDetails on o.OrderId equals od.OrderId
                                join p in _context.Products on od.ProductId equals p.ProductId
                                join s in _context.Stores on o.StoreId equals s.StoreId
                                where s.StoreId == 1
                                group new { c, o, r, od, p } by r.ReviewId into g
                                orderby g.Min(x => x.r.ReviewTime)
                                select new
                                {
                                    ReviewId = g.Key,
                                    OrderId = g.First().o.OrderId,
                                    CustomerId = g.First().c.CustomerId,
                                    CustomerName = g.First().c.CustomerName,
                                    ReviewTime = g.Min(x => x.r.ReviewTime),
                                    ReviewContent = g.First().r.ReviewContent,
                                    ReviewRating = g.First().r.ReviewRating,
                                    StoreReplyContent = g.First().r.StoreReplyContent,
                                    ProductName = string.Join("、 ", g.Select(x => x.p.ProductName)),
                                    StoreId = g.First().o.StoreId
                                });
            if (!String.IsNullOrEmpty(searchString))
            {
                queryReviews = queryReviews.Where(s => s.CustomerName.Contains(searchString)
                                       || s.ProductName.Contains(searchString));
            }
            ViewBag.customers=queryReviews.ToList();
            return View();
        }



    }
}
