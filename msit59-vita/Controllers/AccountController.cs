using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;
using System.Threading.Tasks;
using msit59_vita.Models.ViewModels;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace msit59_vita.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<VitaUser> _userManager;
        private readonly SignInManager<VitaUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private VitaContext _context;

        public AccountController(UserManager<VitaUser> userManager, SignInManager<VitaUser> signInManager, ILogger<AccountController> logger, VitaContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                VitaUser user = new VitaUser {
                    UserName = model.CustomerEmail,
                    VitaUserName = model.CustomerNickName ?? model.CustomerName,
                    Email = model.CustomerEmail,
                    PhoneNumber = model.CustomerCellPhone,
                };
                var result = await _userManager.CreateAsync(user, model.CustomerPassword);

                if (result.Succeeded)
                {
                    // await _signInManager.SignInAsync(user, isPersistent: false);

                    Customer customer = new Customer
                    {
                        CustomerName = model.CustomerName,
                        CustomerNickName = model.CustomerNickName,
                        CustomerEmail = model.CustomerEmail,
                        CustomerCellPhone = model.CustomerCellPhone,
                        CustomerPassword = user.PasswordHash!
                    };
                    _context.Customers.Add(customer);
                    _context.SaveChanges();

                    
                    // 創建自定義聲明
                    var claims = new List<Claim>
                    {
                        new Claim("VitaUserName", user.VitaUserName ?? ""),
                    };
                    // 添加聲明到用戶
                    var claimResult = await _userManager.AddClaimsAsync(user, claims);
                    if (claimResult.Succeeded)
                    {
                        // 更新當前用戶的 ClaimsPrincipal
                        var userIdentity = (ClaimsIdentity)User.Identity!;
                        userIdentity.AddClaims(claims);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return Redirect("/Home");
                    }
                    
                    return Redirect("/Account/Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("createError", error.Description);
                }
                Dictionary<string, string[]> modelStateDictionaryWithErr = ModelState.ToDictionary(
                    x => x.Key,
                    x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                ViewBag.ModelState = modelStateDictionaryWithErr;
                return View();
            }

            Dictionary<string, string[]> modelStateDictionary = ModelState.ToDictionary(
                    x => x.Key,
                    x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            ViewBag.ModelState = modelStateDictionary;
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                VitaUser? user = await _userManager.FindByEmailAsync(model.CustomerEmail);
                if (user != null) {
                    var result = await _signInManager.PasswordSignInAsync(user, model.CustomerPassword, false, false);
                    if (result.Succeeded)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim("VitaUserName", user.VitaUserName ?? ""),
                        };

                        var claimResult = await _userManager.AddClaimsAsync(user, claims);
                        if (claimResult.Succeeded)
                        {
                            var userIdentity = (ClaimsIdentity)User.Identity!;
                            userIdentity.AddClaims(claims);
                            return RedirectToAction("Index","Home");
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    ViewBag.ErrorMessage = "密碼錯誤，請重試。";
                    return View();
                } else
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
