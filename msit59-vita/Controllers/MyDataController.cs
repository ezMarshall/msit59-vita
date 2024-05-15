using Microsoft.AspNetCore.Mvc;

namespace msit59_vita.Controllers
{
    public class MyDataController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
