using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using msit59_vita.Models;


namespace msit59_vita.Controllers
{



    public class FavoriteStoresController : Controller
    {

        private VitaContext _context;

        public FavoriteStoresController(VitaContext context)
        {
            _context = context;
        }


        /*
         * 取得使用者id CustomerID
         * 經由id查找資料 店家資料
         * 營業時間
         * 平均星級+評論數
         */

        public IActionResult Index( )            
        {
            ViewBag.CustomerID = 1;
            var query = from o in _context.Favorites
                        join s in _context.Stores on o.StoreId equals s.StoreId
                        where o.CustomerId == 1
                        select s;

            //var query = from o in _context.Favorites
            //            join s in _context.Stores on o.StoreId equals s.StoreId
            //            join so in _context.StoreOpeningHours on o.StoreId equals so.StoreId
            //            into groupjoin
            //            select groupjoin;
            //var obj = _context.Stores.ToList();
            var obj = query.ToList();
            return View(obj);
        }

    }
}
