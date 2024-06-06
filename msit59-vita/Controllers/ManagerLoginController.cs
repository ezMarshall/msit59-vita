using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models.ViewModels;
using msit59_vita.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace msit59_vita.Controllers
{
    public class ManagerLoginController : Controller
    {
        private readonly UserManager<VitaUser> _userManager;
        private readonly SignInManager<VitaUser> _signInManager;
        private readonly ILogger<ManagerLoginController> _logger;

        public ManagerLoginController(UserManager<VitaUser> userManager, SignInManager<VitaUser> signInManager, ILogger<ManagerLoginController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ManagerLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                VitaUser? user = await _userManager.FindByNameAsync(model.StoreAccountNumber);
                if (user != null && !user.IsCustomer)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.StorePassword, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "ManagerHome");
                    }
                    ViewBag.ErrorMessage = "密碼錯誤，請重試。";
                    return View();
                }
                else
                {
                    ViewBag.ErrorMessage = "用戶 Email不存在";
                    return View();
                }
            }
            ViewBag.ErrorMessage = "請輸入正確格式";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                await HttpContext.SignOutAsync();
                return Content("ok");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "發生異常於 {Method}", nameof(Logout));
                return Content("發生未預期的錯誤,請稍後再試。");
            }
        }
    }
}
