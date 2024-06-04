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
		public IActionResult Payment()
        {
			//先判段使用者是否有登入，若無登入則導向登入頁面


			// 取得目前的訂單資料，透過購物車資料表查詢

            return View();
        }
    }
}
