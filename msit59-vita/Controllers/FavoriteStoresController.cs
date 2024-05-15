using Microsoft.AspNetCore.Mvc;

namespace msit59_vita.Controllers
{
    public class FavoriteStoresController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
