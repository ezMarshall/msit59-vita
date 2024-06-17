using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;

namespace msit59_vita.Controllers
{
    public class TransactionController : Controller
    {

        private readonly VitaContext _context;

        public TransactionController(VitaContext context)
        {
            _context = context;
        }


        public async Task<ActionResult> InitTransactionRecord() 
        {
            var query = from t in _context.

            return await Ok();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
