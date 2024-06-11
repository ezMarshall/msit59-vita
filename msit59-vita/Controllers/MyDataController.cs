using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;

namespace msit59_vita.Controllers
{
    public class MyDataController : Controller
    {
        private VitaContext _context;
        private readonly SignInManager<VitaUser> _signInManager;
        private readonly UserManager<VitaUser> _userManager;

        private int _customerId = 31;

        public MyDataController(VitaContext context, SignInManager<VitaUser> signInManager, UserManager<VitaUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        //測試用
        public IActionResult chat() 
        { 
            return View(); 
        }
        public object GetUserData()
        {

            //取得使用者ID
            var queryCustomerID = from c in _context.Customers
                                  where c.CustomerEmail == User.Identity.Name
                                  select c.CustomerId;
            _customerId = queryCustomerID.Single();

            var queryCustomerData = from c in _context.Customers
                                    where c.CustomerId == _customerId
                                    select new
                                    {
                                        c.CustomerName,
                                        c.CustomerLocalPhone,
                                        c.CustomerEmail,
                                        c.CustomerEinvoiceNumber,
                                        c.CustomerNickName,
                                        c.CustomerId,
                                        c.CustomerCellPhone,
                                        c.CustomerAddressCity,
                                        c.CustomerAddressDetails,
                                        c.CustomerAddressDistrict
                                    };
            return queryCustomerData.First();
        }

        /*
         * 經使用者id取得資料
         * 
         */
        public IActionResult Index()
        {
            //未登入 倒回首頁
            if (!User.Identity?.IsAuthenticated?? false)
            {
                return Redirect("/");
            }
            return View(GetUserData());
        }

        /*
         * 修改資料
         * 
         * 
         */
        public async Task<IActionResult> Edit(int id,string originPassword,string CustomerPassword, string CustomerName,string CustomerLocalPhone,string CustomerEmail,string CustomerEinvoiceNumber,string CustomerNickName,string CustomerCellPhone,string CustomerAddressCity,string CustomerAddressDetails,string CustomerAddressDistrict)
        {

            Customer customer = _context.Customers.Find(id);
            VitaUser user = await _userManager.FindByEmailAsync(CustomerEmail);
            var result = await _signInManager.PasswordSignInAsync(user, originPassword, false, false);

            //密碼不一樣
            if (!result.Succeeded)
            {
                Console.WriteLine("密碼錯誤");
                ViewBag.Invalid = new
                {
                    originPassword="密碼錯誤"
                };
                return View("index", GetUserData());
            }

            await _userManager.ChangePasswordAsync(user, originPassword, String.IsNullOrEmpty(CustomerPassword) ? customer.CustomerPassword.Trim() : CustomerPassword);
            //必填項目
            customer.CustomerPassword = String.IsNullOrEmpty(CustomerPassword) ? customer.CustomerPassword.Trim() : CustomerPassword;
            customer.CustomerName = String.IsNullOrEmpty(CustomerName) ? customer.CustomerName: CustomerName;
            //customer.CustomerNickName = String.IsNullOrEmpty(CustomerNickName) ? customer.CustomerNickName : CustomerNickName;
            customer.CustomerNickName = CustomerNickName;
            customer.CustomerLocalPhone = CustomerLocalPhone;
            customer.CustomerCellPhone = CustomerCellPhone;
            customer.CustomerAddressCity = CustomerAddressCity;
            customer.CustomerAddressDistrict = CustomerAddressDistrict;
            customer.CustomerAddressDetails = CustomerAddressDetails;
            customer.CustomerEinvoiceNumber = CustomerEinvoiceNumber;
            _context.SaveChanges();

            return Redirect("/MyData");
        }
    }
}
