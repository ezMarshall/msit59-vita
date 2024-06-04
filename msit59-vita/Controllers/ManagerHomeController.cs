using Microsoft.AspNetCore.Mvc;

namespace msit59_vita.Controllers
{
    public class ManagerHomeController : Controller
    {
        public IActionResult Index()
        {
            if (!User.Identity?.IsAuthenticated ?? false) //只是多加個驚嘆號為了能夠看到登入頁面
            {
                return View();
            }
            else {
                return Redirect("Account/Login/");
            }
           
        }
    }
}
