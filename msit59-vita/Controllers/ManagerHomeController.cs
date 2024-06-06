using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace msit59_vita.Controllers
{
    public class ManagerHomeController : Controller
    {
        private readonly VitaContext _context;
        private readonly DateTime _todayDate = new DateTime(2024, 06, 02);
        private readonly  int StoreId = 1;

        public ManagerHomeController(VitaContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!User.Identity?.IsAuthenticated ?? false) //只是多加個驚嘆號為了能夠看到登入頁面
            {


                //今日訂單狀態
                var orderCountsByStatus = _context.Orders
               .Where(o => o.OrderTime.Date ==  _todayDate && o.StoreId == StoreId)
               .GroupBy(o => o.CustomerOrderStatus)
               .Select(g => new
               {
                   OrderStatus = g.Key,
                   OrderCount = g.Count()
               })
               .ToList();

                int [ ] OrderStatus = new int[5];
                // 6種狀態:
                //0: 待接單
                //1: 製作中
                //2: 配送中或 等待自取  
                //3: 已完成
                //4: 退單
                //5: 已取消   ->4, 5=合併在同一種狀態顯示

                for ( int i = 0; i < 5; i++)
                {
                    OrderStatus[i] = orderCountsByStatus.FirstOrDefault(item => item.OrderStatus == i)?.OrderCount ?? 0;
                }
                OrderStatus[4] = orderCountsByStatus.Where(item => item.OrderStatus == 4 || item.OrderStatus == 5).Sum(item => item.OrderCount);            

                ViewBag.OrderStatus = OrderStatus;


                //今日營業額、銷售數量
                var order = from o in _context.Orders
                            join os in _context.OrderDetails on o.OrderId equals os.OrderId
                            join p in _context.Products on os.ProductId equals p.ProductId
                            join c in _context.ProductCategories on p.ProductId equals c.CategoryId
                            where o.OrderTime.Date == _todayDate && o.StoreId == StoreId && o.CustomerOrderStatus == 3             //已完成訂單才算數
                            select new {
                                TotalAmount = os.Quantity * os.UnitPrice,
                                Quantity = os.Quantity,
                                UnitPrice = os.UnitPrice,
                                CategoryId = c.CategoryId,
                                CategoryName = c.CategoryName,
                                ProductId = os.ProductId,
                                ProductName = p.ProductName
                            };

                ViewBag.TodayRevenue = Convert.ToInt32(order.Sum(o => o.TotalAmount));
                ViewBag.TodayQuantity = order.Where(o => o.CategoryId== 1).Sum(o => o.Quantity);


                //本週評論數量
                var comment = from r in _context.Reviews
                              join o in _context.Orders on r.OrderId equals o.OrderId
                              where r.ReviewTime.Date > _todayDate.AddDays(-7) && o.StoreId == StoreId
                              select new
                              {
                                  ReviewId = r.ReviewId
                              };
                ViewBag.NumComment = comment.Count();
                ViewBag.Last7DaysDate = _todayDate.AddDays(-7).ToString("yyyy-MM-dd") ;
                ViewBag.TodayDate = _todayDate.Date.ToString("yyyy-MM-dd");


                return View();
            }
            else {
                return Redirect("Account/Login/");
            }
           
        }

    

    }
}
