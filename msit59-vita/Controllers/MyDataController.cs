using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;

namespace msit59_vita.Controllers
{
    public class MyDataController : Controller
    {
        private VitaContext _context ;

        private int _customerId = 1;

        public MyDataController(VitaContext context) 
        { 
            _context = context;
        }

        /*
         * 經使用者id取得資料
         * 
         */
        public IActionResult Index()
        {
            ViewBag.CustomerID = _customerId;





            return View();
        }
    }
}
