using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using msit59_vita.Models;
using System.Globalization;
using static msit59_vita.Models.ManagerOrders;


namespace msit59_vita.Controllers
{
    public class ManagerOrdersController : Controller
    {
        private readonly VitaContext _context;

        public ManagerOrdersController(VitaContext context)
        {
            _context = context;
        }
        public IActionResult ManagerOrders(int currentPage = 1)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var viewModel = GetOrders(currentPage);
                return View(viewModel);
            }else{
                return Redirect("/ManagerLogin");
            }
        }


        // 點擊分頁時
        public IActionResult GetOrdersTable(int currentPage = 1)
        {
            var viewModel = GetOrders(currentPage);
            return PartialView("_OrdersTable", viewModel);
        }

        public ManagerOrders GetOrders(int currentPage)
        {
            int maxRows = 10; //每頁幾列'
            var queryOrders = from o in _context.Orders
                              orderby o.CustomerOrderStatus, o.OrderTime descending
                              where o.StoreId == 1
                              select new OrderViewModel
                              {
                                  StoreId = o.StoreId,
                                  OrderId = o.OrderId,
                                  OrderTime = o.OrderTime,
                                  OrderPayment = o.OrderPayment,
                                  OrderDeliveryVia = o.OrderDeliveryVia,
                                  CustomerOrderStatus = o.CustomerOrderStatus,
                                  CustomerId = o.CustomerId,
                              };
            var orders = queryOrders.Skip((currentPage - 1) * maxRows).Take(maxRows).ToList();
            var viewModel = new ManagerOrders
            {
                Orders = orders
            };
            double pageCount = (double)((decimal)queryOrders.ToList().Count() / Convert.ToDecimal(maxRows));
            viewModel.PageCount = (int)Math.Ceiling(pageCount); //總共頁數
            viewModel.CurrentPageIndex = currentPage; //目前所在分頁
            viewModel.MaxRows = maxRows;
            return viewModel;
        }

        // 篩選
        [HttpPost]
        public PartialViewResult FilterOrders(string searchString, string deliveryVia, string orderStatus, string startDate, string endDate, int currentPage = 1)
        {

            int? orderStatusInt = null;
            if (!string.IsNullOrEmpty(orderStatus))
            {
                if (int.TryParse(orderStatus, out int statusValue))
                {
                    orderStatusInt = statusValue;
                }
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
            var queryOrders = from o in _context.Orders
                              orderby o.CustomerOrderStatus,o.OrderTime descending
                              where o.StoreId == 1
                              select new OrderViewModel
                              {
                                  StoreId = o.StoreId, 
                                  OrderId = o.OrderId,
                                  OrderTime = o.OrderTime,
                                  OrderPayment = o.OrderPayment,
                                  OrderDeliveryVia = o.OrderDeliveryVia,
                                  CustomerOrderStatus = o.CustomerOrderStatus,
                                  OrderFinishedTime= o.OrderFinishedTime,
                              };
                              

            // 根據搜尋字串過濾
            if (!string.IsNullOrEmpty(searchString))
            {
                queryOrders = queryOrders.Where(o => o.OrderId.ToString().Contains(searchString));
            }

            // 根據訂單狀態過濾
            if (orderStatusInt.HasValue)
            {
                switch (orderStatusInt.Value)
                {
                    case 0: // 已完成
                        queryOrders = queryOrders.Where(o => o.CustomerOrderStatus == 3 || o.CustomerOrderStatus == 5);
                        break;
                    case 1: // 未完成
                        queryOrders = queryOrders.Where(o => o.CustomerOrderStatus == 0 || o.CustomerOrderStatus == 1 || o.CustomerOrderStatus == 2);
                        break;
                    case 2: // 已退單
                        queryOrders = queryOrders.Where(o => o.CustomerOrderStatus == 4);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(deliveryVia))
            {
                bool isDelivery = (deliveryVia == "1"); // "1" 表示外送
                queryOrders = queryOrders.Where(o => o.OrderDeliveryVia == isDelivery);
            }

            // 根據發佈時間區間過濾
            if (startDateParsed.HasValue && endDateParsed.HasValue)
            {
                queryOrders = queryOrders.Where(o => o.OrderTime >= startDateParsed.Value && o.OrderTime <= endDateParsed.Value);
            }

            var totalCount = queryOrders.Count(); // 總記錄數
            int maxRows = 10; // 每頁行數

            var reviewsList = queryOrders
            .Skip((currentPage - 1) * maxRows)
            .Take(maxRows)
            .ToList();

            // 計算實際的當前頁碼
            int actualCurrentPage = (currentPage - 1) * maxRows < totalCount
                ? currentPage
                : (int)Math.Ceiling((double)totalCount / maxRows);
            var viewModel = new ManagerOrders
            {
                Orders = reviewsList.Cast<OrderViewModel>().ToList(),
                CurrentPageIndex = actualCurrentPage,
                PageCount = (int)Math.Ceiling((double)totalCount / maxRows),
                MaxRows = maxRows,
                TotalCount = totalCount // 新增：返回總記錄數
            };

            return PartialView("_OrdersTable", viewModel);
        }

        // 更改訂單狀態
        [HttpPost]
        public IActionResult ChangeOrderStatus(int orderId, string action)
        {
            var order = _context.Orders.Find(orderId);
            if (order == null)
            {
                return NotFound();
            }

            if (action == "接單")
            {
                // 查詢訂單詳情
                var orderDetails = _context.OrderDetails
                    .Where(od => od.OrderId == orderId)
                    .ToList();

                foreach (var detail in orderDetails)
                {
                    var product = _context.Products.Find(detail.ProductId);
                    // 檢查庫存量是否足夠
                    if (product.ProductUnitsInStock < detail.Quantity)
                    {
                        // 處理庫存不足的情況，例如返回錯誤訊息
                        return BadRequest("庫存不足");
                    }

                    // 減庫存量
                    product.ProductUnitsInStock -= detail.Quantity;
                }

                // 更新訂單狀態
                order.CustomerOrderStatus = 1; // 製作中
            }
            else if (action == "出餐")
            {
                order.CustomerOrderStatus = 2; // 配送中或等待自取
            }
            else if (action == "退單")
            {
                order.CustomerOrderStatus = 4; // 已退單
                order.OrderFinishedTime = DateTime.Now;
            }
            else if (action == "完成訂單")
            {
                order.CustomerOrderStatus = 3; // 已完成
                order.OrderFinishedTime = DateTime.Now;
            }
            else
            {
                return BadRequest("無效的操作");
            }

            var newMessage = new OrderMessage
            {
                OrderId = orderId,
                MessageInformedTime = DateTime.Now,
                MessageStatus = false,
                MessageContent = order.CustomerOrderStatus
            };
            _context.OrderMessages.Add(newMessage);

            try
            {
                _context.SaveChanges();
                return Ok(new { success = true, message = "訂單狀態已更新，新訊息已創建" });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"更新訂單狀態時發生錯誤: {ex.Message}");
                Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");

                return StatusCode(500, new { success = false, message = "更新訂單狀態時發生錯誤" });
            }
        }
        // Order Details
        public IActionResult OrderDetails(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var queryDetails = (from o in _context.Orders
                                    join c in _context.Customers on o.CustomerId equals c.CustomerId
                                    join od in _context.OrderDetails on o.OrderId equals od.OrderId
                                    join p in _context.Products on od.ProductId equals p.ProductId
                                    where o.OrderId == id
                                    select new
                                    {
                                        OrderEinvoiceNumber = o.OrderEinvoiceNumber,
                                        OrderId = o.OrderId,
                                        CustomerOrderStatus = o.CustomerOrderStatus,
                                        OrderDeliveryVia = o.OrderDeliveryVia,
                                        CustomerName = c.CustomerName,
                                        OrderPhoneNumeber = o.OrderPhoneNumber,
                                        OrderTime = o.OrderTime,
                                        PredictedArrivalTime = o.PredictedArrivalTime,
                                        OrderAddressCity = o.OrderAddressCity,
                                        OrderAddressDistrict = o.OrderAddressDistrict,
                                        OrderAddressDetails = o.OrderAddressDetails,
                                        ProductName = p.ProductName,
                                        Quantity = od.Quantity,
                                        OrderStoreMemo = o.OrderStoreMemo,
                                        OrderUniformInvoiceVia = o.OrderUniformInvoiceVia,
                                        OrderPayment = o.OrderPayment,
                                        ProductInfo = new { ProductName = p.ProductName, Quantity = od.Quantity, UnitPrice = od.UnitPrice }

                                    })
                                .GroupBy(x => x.OrderId)
                                .Select(g => new
                                {
                                    OrderInfo = g.First(),
                                    Products = g.Select(x => x.ProductInfo),
                                    TotalPrice = g.Sum(x => x.ProductInfo.Quantity * x.ProductInfo.UnitPrice)
                                });
                ViewBag.OrderDetails = queryDetails.ToList();
                return View();
            }
            else
            {
                return Redirect("/ManagerLogin");
            }
        }
      
        
        [HttpPost]
        public PartialViewResult FilterCustomerOrderStatus(string orderStatus, string todayDate, int currentPage = 1)
        {
            var TodayDate = DateTime.ParseExact(todayDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var queryOrders = from o in _context.Orders
                              orderby o.CustomerOrderStatus, o.OrderTime descending
                              where o.CustomerOrderStatus == byte.Parse(orderStatus) && o.OrderTime.Date == TodayDate.Date && o.StoreId == 1
                              select new OrderViewModel
                              {
                                  StoreId = o.StoreId,
                                  OrderId = o.OrderId,
                                  OrderTime = o.OrderTime,
                                  OrderPayment = o.OrderPayment,
                                  OrderDeliveryVia = o.OrderDeliveryVia,
                                  CustomerOrderStatus = o.CustomerOrderStatus,
                                  OrderFinishedTime = o.OrderFinishedTime,
                              };

            var totalCount = queryOrders.Count(); // 總記錄數
            int maxRows = 10; // 每頁行數

            var reviewsList = queryOrders
            .Skip((currentPage - 1) * maxRows)
            .Take(maxRows)
            .ToList();

            var viewModel = new ManagerOrders
            {
                Orders = reviewsList.Cast<OrderViewModel>().ToList(),
                CurrentPageIndex = currentPage,
                PageCount = (int)Math.Ceiling((double)totalCount / maxRows),
                MaxRows = maxRows,
                TotalCount = totalCount // 新增：返回總記錄數
            };
            Console.WriteLine("----------------------------------------------------------------------------------------------------------");
            Console.WriteLine(viewModel.PageCount);
            Console.WriteLine(viewModel.TotalCount);
            Console.WriteLine("----------------------------------------------------------------------------------------------------------");

            return PartialView("_OrdersTable", viewModel);
        }

    }



}