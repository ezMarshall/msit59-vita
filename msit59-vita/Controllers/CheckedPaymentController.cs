using Microsoft.AspNetCore.Mvc;

namespace msit59_vita.Controllers
{
    public class CheckedPaymentController : Controller
    {
        //  前台 確認付款頁面
        public IActionResult Index()
        {
            return View();
        }
    }
}
