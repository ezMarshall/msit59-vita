using Microsoft.AspNetCore.Mvc;

namespace msit59_vita.Controllers
{
    //  後台 店家資訊頁面
    public class StoreManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
