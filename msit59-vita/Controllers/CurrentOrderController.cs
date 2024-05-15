using Microsoft.AspNetCore.Mvc;

namespace msit59_vita.Controllers
{
    public class CurrentOrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
