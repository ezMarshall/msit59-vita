using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;
using System;
using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace msit59_vita.Controllers
{
    public class ManagerHomeController : Controller
    {
        private readonly VitaContext _context;
        private readonly DateTime _todayDate = new DateTime(2024, 06, 02);
        private readonly int StoreId = 1;
        private readonly int MainCategoryId = 1;

        public ManagerHomeController(VitaContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false) //只是多加個驚嘆號為了能夠看到登入頁面
            {


                //今日訂單狀態
                var orderCountsByStatus = _context.Orders
               .Where(o => o.OrderTime.Date == _todayDate && o.StoreId == StoreId)
               .GroupBy(o => o.CustomerOrderStatus)
               .Select(g => new
               {
                   OrderStatus = g.Key,
                   OrderCount = g.Count()
               })
               .ToList();

                int[] OrderStatus = new int[6];
                // 6種狀態:
                //0: 待接單
                //1: 製作中
                //2: 配送中或 等待自取  
                //3: 已完成
                //4: 退單
                //5: 已取消   

                for (int i = 0; i < OrderStatus.Length; i++)
                {
                    OrderStatus[i] = orderCountsByStatus.FirstOrDefault(item => item.OrderStatus == i)?.OrderCount ?? 0;
                }

                ViewBag.OrderStatus = OrderStatus;

                //店家商品類別
                var productCategories = (from c in _context.ProductCategories
                                         join p in _context.Products on c.CategoryId equals p.CategoryId
                                         where c.StoreId == StoreId && p.ProductOnSell == true
                                         orderby c.CategoryId
                                         select new
                                         {
                                             CategoryId = c.CategoryId,
                                             CategoryName = c.CategoryName
                                         }).Distinct();
                Console.WriteLine("----------------------------------------------------------------------------------------------------------");


                IEnumerable<string> sortedCategoryNames = productCategories.Select(x => x.CategoryName);
                string[] ProductCategoryList = sortedCategoryNames.ToArray();

                ViewBag.ProductCategoryList = ProductCategoryList;

                for (int i = 0; i < ProductCategoryList.Length; i++)
                {
                    Console.Write(ProductCategoryList[i] + " ");
                }

                Console.WriteLine("----------------------------------------------------------------------------------------------------------");


                //今日營業額、銷售數量
                var order = from o in _context.Orders
                            join os in _context.OrderDetails on o.OrderId equals os.OrderId
                            join p in _context.Products on os.ProductId equals p.ProductId
                            join c in _context.ProductCategories on p.CategoryId equals c.CategoryId
                            where o.OrderTime.Date == _todayDate && o.StoreId == StoreId && o.CustomerOrderStatus == 3             //已完成訂單才算數
                            select new
                            {
                                TotalAmount = os.Quantity * os.UnitPrice,
                                Quantity = os.Quantity,
                                CategoryId = c.CategoryId,
                                CategoryName = c.CategoryName

                            };

                ViewBag.TodayRevenue = string.Format("{0:n0}", Convert.ToInt32(order.Sum(o => o.TotalAmount)));
                ViewBag.TodayQuantity = string.Format("{0:n0}", order.Where(o => o.CategoryId == MainCategoryId).Sum(o => o.Quantity));


                // 依客戶訂單狀態統計銷售金額 -> 圖表依商品類別、訂單狀態顯示
                var orderStatusPerDay = from o in _context.Orders
                                        join os in _context.OrderDetails on o.OrderId equals os.OrderId
                                        join p in _context.Products on os.ProductId equals p.ProductId
                                        join c in _context.ProductCategories on p.CategoryId equals c.CategoryId
                                        where o.OrderTime.Date == _todayDate && o.StoreId == StoreId
                                        orderby c.CategoryId
                                        select new
                                        {
                                            CategoryId = c.CategoryId,
                                            CategoryName = c.CategoryName,
                                            CustomerOrderStatus = o.CustomerOrderStatus,
                                            TotalAmount = os.Quantity * os.UnitPrice
                                        }
                                        into orderWithTotalAmount
                                        group orderWithTotalAmount by new
                                        {
                                            orderWithTotalAmount.CustomerOrderStatus,
                                            orderWithTotalAmount.CategoryId,

                                        } into g
                                        select new
                                        {
                                            OrderStatus = g.Key.CustomerOrderStatus,
                                            CategoryId = g.Key.CategoryId,
                                            TotalAmount = g.Sum(x => x.TotalAmount)
                                        };


                int[,] OrderTotalAmountByCategory = new int[ProductCategoryList.Length, 5];

                for (int i = 0; i < ProductCategoryList.Length; i++)
                {
                    var categoryId = orderStatusPerDay.Select(x => x.CategoryId).Distinct().ElementAt(i);
                    var category = orderStatusPerDay.Where(x => x.CategoryId == categoryId);

                    for (int j = 0; j < 4; j++)
                    {
                        OrderTotalAmountByCategory[i, j] = Convert.ToInt32(category.FirstOrDefault(item => item.OrderStatus == j)?.TotalAmount ?? 0);
                    }
                    OrderTotalAmountByCategory[i, 4] = Convert.ToInt32(category.Where(item => item.OrderStatus == 4 || item.OrderStatus == 5)?.Sum(item => item.TotalAmount) ?? 0);

                }

                ViewBag.OrderTotalAmountByCategory = OrderTotalAmountByCategory;

                Console.WriteLine("----------------------------------------------------------------------------------------------------------");
                int rows = OrderTotalAmountByCategory.GetLength(0);
                int columns = OrderTotalAmountByCategory.GetLength(1);

                //遍历数组并输出每个元素
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        Console.Write(OrderTotalAmountByCategory[i, j] + " ");
                    }
                    Console.WriteLine(); // 在每行结束时换行
                }
                Console.WriteLine("----------------------------------------------------------------------------------------------------------");


                //前週評論數量
                var comment = from r in _context.Reviews
                              join o in _context.Orders on r.OrderId equals o.OrderId
                              where r.ReviewTime.Date > _todayDate.AddDays(-7) && o.StoreId == StoreId
                              select new
                              {
                                  ReviewId = r.ReviewId
                              };
                ViewBag.NumComment = comment.Count();
                ViewBag.Last7DaysDate = _todayDate.AddDays(-7).ToString("yyyy-MM-dd");
                ViewBag.TodayDate = _todayDate.Date.ToString("yyyy-MM-dd");


                //前週評論馬賽克圖
                var weeklycommentbyproduct = from o in _context.Orders
                                             join r in _context.Reviews on o.OrderId equals r.OrderId
                                             join od in _context.OrderDetails on o.OrderId equals od.OrderId
                                             join p in _context.Products on od.ProductId equals p.ProductId
                                             join c in _context.ProductCategories on p.CategoryId equals c.CategoryId
                                             where r.ReviewTime.Date > _todayDate.AddDays(-7) && o.StoreId == StoreId && c.CategoryId == MainCategoryId
                                             select new
                                             {
                                                 ReviewTime = r.ReviewTime.ToString("yyyy-MM-dd"),
                                                 ProductName = p.ProductName,
                                                 ReviewRating = r.ReviewRating,
                                                 TotalAmount = od.Quantity * od.UnitPrice
                                             } into weeklycomment
                                             group weeklycomment by new
                                             {
                                                 weeklycomment.ProductName
                                             } into g
                                             orderby g.Sum(x => x.TotalAmount) descending
                                             select new
                                             {
                                                 ProductName = g.Key.ProductName,
                                                 ReviewRating = g.OrderByDescending(x => x.ReviewRating).Select(x => x.ReviewRating),
                                                 TotalAmount = g.Sum(x => x.TotalAmount)
                                             };


                Console.WriteLine("----------------------------------------------------------------------------------------------------------");
                foreach (var item in weeklycommentbyproduct)
                {
                    Console.WriteLine($"Product Name: {item.ProductName}");
                    Console.WriteLine($"Total Amount: {item.TotalAmount}");
                    Console.WriteLine($"Review Ratings:");
                    foreach (var rating in item.ReviewRating)
                    {
                        Console.WriteLine($" - {rating}");
                    }
                    Console.WriteLine("----------------------------------");
                }
                Console.WriteLine("----------------------------------------------------------------------------------------------------------");

                string[] WeeklyProductNames = new string[weeklycommentbyproduct.Count()];
                for (int i = 0; i < weeklycommentbyproduct.Count(); i++)
                {
                    WeeklyProductNames[i] = weeklycommentbyproduct.ElementAt(i).ProductName;
                }

                var weeklycommentbyproductandrating = from o in _context.Orders
                                                      join r in _context.Reviews on o.OrderId equals r.OrderId
                                                      join od in _context.OrderDetails on o.OrderId equals od.OrderId
                                                      join p in _context.Products on od.ProductId equals p.ProductId
                                                      join c in _context.ProductCategories on p.CategoryId equals c.CategoryId
                                                      where r.ReviewTime.Date > _todayDate.AddDays(-7) && o.StoreId == StoreId && c.CategoryId == MainCategoryId
                                                      select new
                                                      {
                                                          ReviewTime = r.ReviewTime.ToString("yyyy-MM-dd"),
                                                          ProductName = p.ProductName,
                                                          ReviewRating = r.ReviewRating,
                                                          TotalAmount = od.Quantity * od.UnitPrice
                                                      } into weeklycomment
                                                      group weeklycomment by new
                                                      {
                                                          weeklycomment.ProductName,
                                                          weeklycomment.ReviewRating
                                                      } into g
                                                      select new
                                                      {
                                                          ProductName = g.Key.ProductName,
                                                          ReviewRating = g.Key.ReviewRating,
                                                          TotalAmount = g.Sum(x => x.TotalAmount)
                                                      };

                ArrayList WeeklyReviewByProductAndRating = new ArrayList();

                for (int i = 0; i < weeklycommentbyproduct.Count(); i++)
                {
                    ArrayList productInfo = new ArrayList();
                    productInfo.Add(weeklycommentbyproduct.ElementAt(i).ProductName);
                    for (int j = 0; j < 5; j++)
                    {
                        var temp = weeklycommentbyproductandrating.Where(x => x.ProductName == weeklycommentbyproduct.ElementAt(i).ProductName && x.ReviewRating == (5 - j));
                        if (temp != null)

                        {
                            productInfo.Add(temp.Sum(x => x.TotalAmount));
                        }
                        else
                        {
                            productInfo.Add(0);
                        }
                    }
                    WeeklyReviewByProductAndRating.Add(productInfo);
                }

                Console.WriteLine("***********************************************************************************************************************************");
                for (int i = 0; i < WeeklyReviewByProductAndRating.Count; i++)
                {
                    ArrayList productInfo = (ArrayList)WeeklyReviewByProductAndRating[i];
                    for (int j = 0; j < productInfo.Count; j++)
                    {
                        Console.Write(productInfo[j] + " ");
                    }
                    Console.WriteLine(); // 换行
                }

                Console.WriteLine("***********************************************************************************************************************************");

                ViewBag.WeeklyReviewByProductAndRating = WeeklyReviewByProductAndRating;


                //月交易金額統計
                var monthOrder = from o in _context.Orders
                                 join os in _context.OrderDetails on o.OrderId equals os.OrderId
                                 join p in _context.Products on os.ProductId equals p.ProductId
                                 join c in _context.ProductCategories on p.CategoryId equals c.CategoryId
                                 where o.OrderTime.Month == _todayDate.AddMonths(-1).Month && o.StoreId == StoreId
                                 select new
                                 {
                                     TotalAmount = os.Quantity * os.UnitPrice,
                                     Quantity = os.Quantity,
                                     CategoryId = c.CategoryId,
                                     CategoryName = c.CategoryName
                                 };


                ViewBag.MonthlyRevenue = string.Format("{0:n0}", Convert.ToInt32(monthOrder.Sum(o => o.TotalAmount)));
                ViewBag.MonthlyQuantity = string.Format("{0:n0}", monthOrder.Where(o => o.CategoryId == MainCategoryId).Sum(o => o.Quantity));

                // 月商品銷售橫條圖
                var monthlyproductsales = (from o in _context.Orders
                                           join od in _context.OrderDetails on o.OrderId equals od.OrderId
                                           join p in _context.Products on od.ProductId equals p.ProductId
                                           join c in _context.ProductCategories on p.CategoryId equals c.CategoryId
                                           where o.OrderTime.Month == _todayDate.AddMonths(-1).Month && o.StoreId == StoreId && c.CategoryId == MainCategoryId
                                           select new
                                           {
                                               ProductId = p.ProductId,
                                               ProductName = p.ProductName,
                                               Quantity = od.Quantity,
                                               TotalAmount = od.Quantity * od.UnitPrice
                                           } into monthlyproduct
                                           group monthlyproduct by new
                                           {
                                               monthlyproduct.ProductName
                                           } into g
                                           select new
                                           {
                                               ProductName = g.Key.ProductName,
                                               TotalAmount = g.Sum(x => x.TotalAmount),
                                               Quantity = g.Sum(x => x.Quantity)
                                           }).OrderByDescending(x => x.TotalAmount);

                int[] MonthlyProductSales = new int[monthlyproductsales.Count()];
                string[] MonthlyProductNames = new string[monthlyproductsales.Count()];

                for (int i = 0; i < monthlyproductsales.Count(); i++)
                {
                    MonthlyProductSales[i] = Convert.ToInt32(monthlyproductsales.ElementAt(i).TotalAmount);
                    MonthlyProductNames[i] = monthlyproductsales.ElementAt(i).ProductName;
                }

                ViewBag.MonthlyProductSales = MonthlyProductSales;
                ViewBag.MonthlyProductNames = MonthlyProductNames;
                ViewBag.SumMonthlyProductSales = MonthlyProductSales.Sum();


                Console.WriteLine("----------------------------------------------------------------------------------------------------------");

                for (int i = 0; i < MonthlyProductSales.Length; i++)
                {
                    Console.Write(MonthlyProductNames[i] + " " + MonthlyProductSales[i] + " ");
                }
                Console.WriteLine(ViewBag.SumMonthlyProductSales);

                Console.WriteLine("----------------------------------------------------------------------------------------------------------");


                // 月客戶銷售甜甜圈圖
                var monthlycustomersales = (from o in _context.Orders
                                            join od in _context.OrderDetails on o.OrderId equals od.OrderId

                                            join c in _context.Customers on o.CustomerId equals c.CustomerId
                                            where o.OrderTime.Month == _todayDate.AddMonths(-1).Month && o.StoreId == StoreId
                                            select new
                                            {
                                                CustomerName = c.CustomerName,
                                                TotalAmount = od.Quantity * od.UnitPrice
                                            } into monthlycustomer
                                            group monthlycustomer by new
                                            {
                                                monthlycustomer.CustomerName
                                            } into g
                                            select new
                                            {
                                                CustomerName = g.Key.CustomerName,
                                                TotalAmount = g.Sum(x => x.TotalAmount)
                                            }).OrderByDescending(x => x.TotalAmount);

                int[] MonthlyCustomerSales = new int[monthlycustomersales.Count()];
                string[] MonthlyCustomerNames = new string[monthlycustomersales.Count()];

                for (int i = 0; i < monthlycustomersales.Count(); i++)
                {
                    MonthlyCustomerSales[i] = Convert.ToInt32(monthlycustomersales.ElementAt(i).TotalAmount);
                    MonthlyCustomerNames[i] = monthlycustomersales.ElementAt(i).CustomerName;
                }

                ViewBag.MonthlyCustomerSales = MonthlyCustomerSales;
                ViewBag.MonthlyCustomerNames = MonthlyCustomerNames;
                ViewBag.SumMonthlyCustomerSales = MonthlyCustomerSales.Sum();


                Console.WriteLine("----------------------------------------------------------------------------------------------------------");

                for (int i = 0; i < MonthlyCustomerSales.Length; i++)
                {
                    Console.Write(MonthlyCustomerNames[i] + " " + MonthlyCustomerSales[i] + " ");

                }

                Console.WriteLine("----------------------------------------------------------------------------------------------------------");

                // 月每日營收、訂單數趨勢圖
                var monthlysalesperday = (from o in _context.
                                          join od in _context.OrderDetails on o.OrderId equals od.OrderId
                                          where o.OrderTime.Month == _todayDate.AddMonths(-1).Month && o.StoreId == StoreId
                                          select new
                                          {
                                              OrderDate = o.OrderTime.Date,
                                              TotalAmount = od.Quantity * od.UnitPrice,
                                              OrderID = o.OrderId
                                          } into monthlysale
                                          group monthlysale by new
                                          {
                                              monthlysale.OrderDate
                                          } into g
                                          select new
                                          {
                                              OrderDate = g.Key.OrderDate,
                                              TotalAmount = g.Sum(x => x.TotalAmount),
                                              DistinctOrderIDCount = g.Select(x => x.OrderID).Distinct().Count()
                                          }).OrderBy(x => x.OrderDate);


                string[] MonthlyOrderDates = new string[monthlysalesperday.Count()];
                int[] MonthlyTotalAmountPerDay = new int[monthlysalesperday.Count()];
                int[] MonthlyOrderIdPerDay = new int[monthlysalesperday.Count()];

                for (int i = 0; i < monthlysalesperday.Count(); i++)
                {
                    MonthlyOrderDates[i] = monthlysalesperday.ElementAt(i).OrderDate.ToString("yyyy-MM-dd");
                    MonthlyTotalAmountPerDay[i] = Convert.ToInt32(monthlysalesperday.ElementAt(i).TotalAmount);
                    MonthlyOrderIdPerDay[i] = Convert.ToInt32(monthlysalesperday.ElementAt(i).DistinctOrderIDCount);
                }

                ViewBag.MonthlyTotalAmountPerDay = MonthlyTotalAmountPerDay;
                ViewBag.MonthlyOrderIdPerDay = MonthlyOrderIdPerDay;
                ViewBag.MonthlyOrderDates = MonthlyOrderDates;

                Console.WriteLine("----------------------------------------------------------------------------------------------------------");

                for (int i = 0; i < MonthlyOrderDates.Length; i++)
                {
                    Console.Write(MonthlyOrderDates[i] + " " + MonthlyOrderIdPerDay[i] + " " + MonthlyTotalAmountPerDay[i] + " ");

                }

                Console.WriteLine("----------------------------------------------------------------------------------------------------------");


                return View();
            }
            else
            {
                return Redirect("/ManagerLogin");
            }

        }


    }
}
