using Microsoft.AspNetCore.Mvc;

namespace msit59_vita.Controllers
{
    public class FaqController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
