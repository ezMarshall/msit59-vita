using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;

namespace msit59_vita.Controllers
{
    public class RegisterController : Controller
    {
        private VitaContext _context;
        public RegisterController(VitaContext context) {
            _context = context;
        }

        public IActionResult Register()
        {
            ViewBag.isNameValid = true;
            ViewBag.isEmailValid = true;
            ViewBag.isCellPhoneValid = true;
            ViewBag.isPasswordValid = true;

            return View();
        }

        [HttpPost]
        public IActionResult IsEmailAvailable(string CustomerEmail)
        {
            bool isAvailable = !_context.Customers.Any(u => u.CustomerEmail == CustomerEmail);
            return Json(isAvailable);
        }

        [HttpPost]
        public IActionResult Register(Customer model)
        {
            //驗證是否成功
            if (ModelState.IsValid)
            {
                //用戶是否重複
                if (_context.Customers.Any(u => u.CustomerEmail == model.CustomerEmail))
                {
                    ModelState.AddModelError("CustomerEmail", "用戶Email已註冊");
                    Dictionary<string, string[]> modelStateDictionary = ModelState.ToDictionary(
                        x => x.Key,
                        x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                    ViewBag.ModelState = modelStateDictionary;

                    return View(model);
                }

                _context.Customers.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            else
            {
                Dictionary<string, string[]> modelStateDictionary = ModelState.ToDictionary(
                    x => x.Key,
                    x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

                ViewBag.ModelState = modelStateDictionary;
                return View(model);
            }
        }
    }
}
