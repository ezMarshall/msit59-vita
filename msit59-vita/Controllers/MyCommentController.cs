﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using msit59_vita.Models;
using System.Globalization;
using System;


namespace msit59_vita.Controllers
{
    public class MyCommentController : Controller
    {

        private VitaContext _context;

        private int _customerId = 31;

        public MyCommentController(VitaContext context)
        {
            _context = context;
        }


        /*
         * 先把資料拿出來,畫面之後再調整
         * 品項數目
         * TODO滾動
         */


        //public async Task<ActionResult<dynamic>> index()
        //{
        //    var review = await _context.Reviews.FindAsync(_customerId);

        //    if (review == null)
        //    {
        //        return NotFound();
        //    }
            
        //    return View(queryComment.ToList());
        //}

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


            var queryComment = from r in _context.Reviews
                               where r.Order.CustomerId == _customerId
                               orderby r.ReviewTime descending
                               select new
                               {
                                   StoreName = r.Order.Store.StoreName,
                                   r.ReviewId,
                                   ReviewRating = Convert.ToDouble(r.ReviewRating),
                                   r.ReviewContent,
                                   ReviewTime = r.ReviewTime.ToString("yyyy/MM/dd HH:mm"),
                                   StoreImage = r.Order.Store.StoreImage,
                                   StoreReplyTime = String.IsNullOrWhiteSpace(r.StoreReplyTime.ToString()) ? "":Convert.ToDateTime(r.StoreReplyTime).ToString("yyyy/MM/dd HH:mm"),
                                   r.StoreReplyContent,
                                   r.OrderId

                               };

            var queryProducts = from r in _context.Reviews
                                join o in _context.Orders on r.OrderId equals o.OrderId
                                join od in _context.OrderDetails on o.OrderId equals od.OrderId
                                where o.CustomerId == _customerId
                                select new
                                {
                                   r.ReviewId,
                                   od.Product.ProductName,
                                   od.UnitPrice,
                                   od.Quantity
                                    
                                };

            ViewBag.customer = queryCustomer.Single();
            ViewBag.products = queryProducts.ToList();
            return View(queryComment.ToList());
        }
    }
}
