﻿using Microsoft.AspNetCore.Mvc;

namespace msit59_vita.Controllers
{
    public class OrdersManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
