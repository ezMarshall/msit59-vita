using Microsoft.AspNetCore.Mvc;

namespace msit59_vita.Controllers
{
    //  後台訂單詳細資訊頁面
    public class OrderDetailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
