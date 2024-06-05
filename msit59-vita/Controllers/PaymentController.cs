using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;

namespace msit59_vita.Controllers
{
    public class PaymentController : Controller
    {
		private readonly VitaContext _context;

		public PaymentController(VitaContext context)
        {
            _context = context;
        }
		public IActionResult Index()
        {
            return View();
        }
    }
}
