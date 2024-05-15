using Microsoft.AspNetCore.Mvc;

namespace msit59_vita.Controllers
{
    public class HistoricalOrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
