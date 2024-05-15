using Microsoft.AspNetCore.Mvc;

namespace msit59_vita.Controllers
{
    //  前台 單一店家頁面 
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
