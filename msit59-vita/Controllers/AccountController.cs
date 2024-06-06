using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;
using System.Threading.Tasks;
using msit59_vita.Models.ViewModels;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

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
                    IsCustomer = true,
                };
                var result = await _userManager.CreateAsync(user, model.CustomerPassword);

                if (result.Succeeded)
                {
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
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Redirect("/Home");
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
                if (user != null && user.IsCustomer) {

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
                            await _signInManager.SignOutAsync();
                            await _signInManager.SignInAsync(user, isPersistent: false);
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

        // google 驗證
        public IActionResult GoogleLogin(string returnUrl = null)
        {
			var redirectUrl = Url.Action("Callback", "Account", new { returnUrl });
			var properties = new AuthenticationProperties { RedirectUri = redirectUrl ?? "/" };
			return Challenge(properties, GoogleDefaults.AuthenticationScheme);
		}

        public async Task<IActionResult> Callback(string returnUrl = null)
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (result.Succeeded)
            {
			    var claims = result.Principal?.Claims;
			    var cName = claims?.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value ?? "用戶";
			    var cEmail = claims?.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value ?? "null";

			    VitaUser? user = await _userManager.FindByEmailAsync(cEmail);
                if (user != null && user.IsCustomer) {
					var myClaims = new List<Claim>
					{
						new Claim("VitaUserName", user.VitaUserName ?? ""),
					};

					var claimResult = await _userManager.AddClaimsAsync(user, myClaims);
					if (claimResult.Succeeded)
					{
						var userIdentity = (ClaimsIdentity)User.Identity!;
						userIdentity.AddClaims(myClaims);
					}
					await _signInManager.SignInAsync(user, isPersistent: false);
					return Redirect("/Home");
				}
				VitaUser newUser = new VitaUser
				{
					UserName = cEmail,
					VitaUserName = cName,
					Email = cEmail,
					IsCustomer = true,
				};
				string randomPassword = GenerateRandomPassword(8);
				var createResult = await _userManager.CreateAsync(newUser, randomPassword);

				if (createResult.Succeeded)
				{
					Customer customer = new Customer
					{
						CustomerName = cName,
						CustomerEmail = cEmail,
					};
					_context.Customers.Add(customer);
					_context.SaveChanges();

					// 創建自定義聲明
					var myClaims = new List<Claim>
					{
						new Claim("VitaUserName", newUser.VitaUserName ?? ""),
					};

					var claimResult = await _userManager.AddClaimsAsync(newUser, myClaims);
					if (claimResult.Succeeded)
                    {
					    var userIdentity = (ClaimsIdentity)User.Identity!;
					    userIdentity.AddClaims(myClaims);
                    }
					await _signInManager.SignInAsync(newUser, isPersistent: false);
					return Redirect("/Home");
				}
			}

			ViewBag.ErrorMessage = "出現未預期錯誤，請用VITA帳號登入";
            return RedirectToAction("Login", "Account");
		}

        // 取亂數
		public static string GenerateRandomPassword(int passwordLength = 12)
		{
			string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
			Random random = new Random();

			char[] chars = new char[passwordLength];
			for (int i = 0; i < passwordLength; i++)
			{
				chars[i] = validChars[random.Next(0, validChars.Length)];
			}
			return new string(chars);
		}
	}
}
