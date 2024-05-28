using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;

namespace msit59_vita.Controllers
{
    //  前台 單一店家頁面 
    public class StoreController : Controller
    {
        private readonly VitaContext _context;
        public StoreController(VitaContext context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
