using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;
using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;
using System.Drawing;
using System.Diagnostics;
using static msit59_vita.Models.ManagerReviews;
using NuGet.Protocol;

namespace msit59_vita.Controllers
{
    public class ManagerCommentsController : Controller
    {
        private readonly VitaContext _context;

        public ManagerCommentsController(VitaContext context)
        {
            _context = context;
        }

        public IActionResult ManagerComments(int currentPage = 1)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var viewModel = GetReviews(currentPage);
                return View(viewModel);
            }
            else
            {
                return Redirect("/ManagerLogin");
            }
        }

        public IActionResult GetReviewsTable(int currentPage = 1)
        {
            var viewModel = GetReviews(currentPage);
            return PartialView("_ReviewsTable", viewModel);
        }

        public ManagerReviews GetReviews(int currentPage)
        {
            // 透過訂單ID 找到 顧客ID
            // 透過顧客ID 找到評論發布人
            // 透過 訂單ID 找到 訂單內容
            // 僅選出店家ID==1的店家
            // 以時間先後排序
            // group by reviewID 避免不同訂購項目分行顯示 (除了訂購品項外 從每個分組中選出第一筆資料)
            // 將訂購品項用頓號隔開
            int maxRows = 10; //每頁幾列'
            var queryReviews = (from c in _context.Customers
                                join o in _context.Orders on c.CustomerId equals o.CustomerId
                                join r in _context.Reviews on o.OrderId equals r.OrderId
                                join od in _context.OrderDetails on o.OrderId equals od.OrderId
                                join p in _context.Products on od.ProductId equals p.ProductId
                                join s in _context.Stores on o.StoreId equals s.StoreId
                                where s.StoreId == 1
                                group new { c, o, r, od, p } by r.ReviewId into g
                                orderby g.Min(x => x.r.ReviewTime)
                                select new ReviewViewModel
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
                                    StoreId = g.First().o.StoreId,
                                });

            var reviews = queryReviews.Skip((currentPage - 1) * maxRows).Take(maxRows).ToList();
            var viewModel = new ManagerReviews
            {
                Reviews = reviews
            };
            double pageCount = (double)((decimal)queryReviews.ToList().Count() / Convert.ToDecimal(maxRows));
            viewModel.PageCount = (int)Math.Ceiling(pageCount); //總共頁數
            viewModel.CurrentPageIndex = currentPage; //目前所在分頁
            viewModel.MaxRows = maxRows;
            return viewModel;
        }

        [HttpPost]
        public IActionResult ManagerComments(Review replyItem)
        {
            var reply = _context.Reviews.Find(replyItem.ReviewId);
            reply.StoreReplyContent = replyItem.StoreReplyContent;
            reply.StoreReplyTime = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("ManagerComments", "ManagerComments");
        }

        public IActionResult FilterReviews(string searchString, string replyStatus, string startDate, string endDate, int currentPage = 1)
        {
            var viewModel = GetFilterReviews(searchString, replyStatus, startDate, endDate, currentPage);
            return PartialView("_ReviewsTable", viewModel);
        }


        public ManagerReviews GetFilterReviews(string searchString, string replyStatus, string startDate, string endDate, int currentPage = 1)
        {
            int? replyStatusInt = null;
            if (!string.IsNullOrEmpty(replyStatus))
            {
                replyStatusInt = int.Parse(replyStatus);
            }

            DateTime? startDateParsed = null;
            DateTime? endDateParsed = null;
            if (!string.IsNullOrEmpty(startDate))
            {
                startDateParsed = DateTime.Parse(startDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                endDateParsed = DateTime.Parse(endDate);
            }

            var queryReviews = (from c in _context.Customers
                                join o in _context.Orders on c.CustomerId equals o.CustomerId
                                join r in _context.Reviews on o.OrderId equals r.OrderId
                                join od in _context.OrderDetails on o.OrderId equals od.OrderId
                                join p in _context.Products on od.ProductId equals p.ProductId
                                join s in _context.Stores on o.StoreId equals s.StoreId
                                where s.StoreId == 1
                                group new { c, o, r, od, p } by r.ReviewId into g
                                orderby g.Min(x => x.r.ReviewTime)
                                select new ReviewViewModel
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
                                    StoreId = g.First().o.StoreId,
                                });

            // 根據搜尋字串過濾
            if (!string.IsNullOrEmpty(searchString))
            {
                queryReviews = queryReviews.Where(r => r.ReviewContent.Contains(searchString) || r.CustomerName.Contains(searchString));
            }

            // 根據回覆狀態過濾
            if (replyStatusInt.HasValue)
            {
                queryReviews = queryReviews.Where(r => (r.StoreReplyContent == null && replyStatusInt == 0) || (r.StoreReplyContent != null && replyStatusInt == 1));
            }

            // 根據發佈時間區間過濾
            if (startDateParsed.HasValue && endDateParsed.HasValue)
            {
                //Console.WriteLine($"你好: {startDateParsed}");
                //Console.WriteLine($"你好: {endDateParsed}");

                queryReviews = queryReviews.Where(r => r.ReviewTime >= startDateParsed.Value && r.ReviewTime <= endDateParsed.Value);
            }
          

            var totalCount = queryReviews.Count(); // 總記錄數
            int maxRows = 10; // 每頁行數

            var reviewsList = queryReviews
                .Skip((currentPage - 1) * maxRows)
                .Take(maxRows)
                .ToList();

            var viewModel = new ManagerReviews
            {
                Reviews = reviewsList.Cast<ReviewViewModel>().ToList(),
                CurrentPageIndex = currentPage,
                PageCount = (int)Math.Ceiling((double)totalCount / maxRows),
                MaxRows = maxRows,
                TotalCount = totalCount // 新增：返回總記錄數
            };

            return viewModel;
        }



    }
}
