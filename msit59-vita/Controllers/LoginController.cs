using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;
using System.Web;

namespace msit59_vita.Controllers
{
    public class LoginController : Controller
    {
        private VitaContext _context;

        public LoginController(VitaContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string CustomerEmail, string CustomerPassword)
        {
            Customer? user = _context.Customers.SingleOrDefault(x => x.CustomerEmail == CustomerEmail);
            if (user != null && user!.CustomerPassword == CustomerPassword)
            {
                Microsoft.AspNetCore.Http.CookieOptions cookieOptions =
                new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddDays(14)
                };
                Response.Cookies.Append("userName", HttpUtility.UrlEncode(user.CustomerName), cookieOptions);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "帳號或密碼輸入錯誤";
            return View();
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("userName");
            return RedirectToAction("Index", "Home");
        }
    }
}
