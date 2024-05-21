using Microsoft.AspNetCore.Mvc;

namespace msit59_vita.Controllers
{
    public class ManagerProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
