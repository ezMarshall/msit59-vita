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
using System.Net.Mail;

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
        #region 註冊
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
                        CustomerPassword = user.PasswordHash!,
                        IsGoogleLogin = false
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
        #endregion

        #region 登入
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
        #endregion

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

        #region google 驗證
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
                        IsGoogleLogin = true
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
        #endregion
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

        #region 忘記密碼
        [HttpPost]
        public async Task<IActionResult> ForgetPwd(EmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                VitaUser? user = await _userManager.FindByEmailAsync(model.CustomerEmail);
                if (user == null)
                {
                    model.Success = false;
                    return Json(model);
                }
                string randomPassword = GenerateRandomPassword(12);
                string newPasswordHash = _userManager.PasswordHasher.HashPassword(user, randomPassword);
                user.PasswordHash = newPasswordHash;
                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    // send email
                    var myAppConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    var UserName = myAppConfig.GetValue<string>("EmailConfig:UserName");
                    var Password = myAppConfig.GetValue<string>("EmailConfig:Password");
                    var Host = myAppConfig.GetValue<string>("EmailConfig:Host");
                    var Port = myAppConfig.GetValue<int>("EmailConfig:Port");
                    var FromEmail = myAppConfig.GetValue<string>("EmailConfig:FromEmail");

                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(FromEmail!);
                    message.To.Add(model.CustomerEmail.ToString());
                    message.Subject = "VITA - 忘記密碼認證信";
                    message.Body = $@"<h3>親愛的會員您好：</h3>
                                      <p>您的帳號 {model.CustomerEmail}</p>
                                      <p>因您忘記密碼，系統發送新密碼為 {randomPassword}</p>
                                      <p>請使用新密碼登入並修改密碼。</p>
                                    ";
                    message.IsBodyHtml = true;

                    SmtpClient smtpClient = new SmtpClient(Host);
                    try
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new System.Net.NetworkCredential(UserName, Password);
                        smtpClient.Host = Host!;
                        smtpClient.Port = Port;
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(message);
                        model.Success = true;
                        return Json(model);
                    }
                    catch (Exception ex)
                    {
                        model.Success = false;
                        model.Message = "出現無法預期的問題，請稍後再試。" + ex.Message;
                        return Json(model);
                    }
                    finally
                    {
                        smtpClient.Dispose();
                    }
                    // send email end
                }

                model.Message = "密碼重設失敗，請稍後再試";
            }
            else
            {
                model.Message = "請輸入正確的電子郵件地址";
            }

            return Json(model);
        }
        #endregion
    }
}
