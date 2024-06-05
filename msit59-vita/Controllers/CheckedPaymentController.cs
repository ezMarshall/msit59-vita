using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;

namespace msit59_vita.Controllers
{
    public class CheckedPaymentController : Controller
    {
		//  前台 確認付款頁面

		private readonly VitaContext _context;

		public CheckedPaymentController(VitaContext context)
		{
			_context = context;
		}
		public IActionResult Index()
        {
			// 取得目前的訂單資料

            return View();
        }
    }
}
