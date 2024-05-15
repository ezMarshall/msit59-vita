using Microsoft.AspNetCore.Mvc;

namespace msit59_vita.Controllers
{
    public class MyCommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
