using Microsoft.AspNetCore.Mvc;
using msit59_vita.Models;
using System.Globalization;

namespace msit59_vita.Controllers
{
    public class MyCommentController : Controller
    {

        private VitaContext _context;

        private int _customerId = 31;

        public MyCommentController(VitaContext context)
        {
            _context = context;
        }


        /*
         * 先把資料拿出來,畫面之後再調整
         * 品項數目
         * TODO滾動
         */


        //public async Task<ActionResult<dynamic>> index()
        //{
        //    var review = await _context.Reviews.FindAsync(_customerId);

        //    if (review == null)
        //    {
        //        return NotFound();
        //    }
            
        //    return View(queryComment.ToList());
        //}

        public IActionResult Index()
        {
            var dtfi = CultureInfo.CurrentCulture.DateTimeFormat;
            var queryComment = from r in _context.Reviews
                               where r.Order.CustomerId == _customerId
                               orderby r.ReviewTime descending
                               select new
                               {
                                   StoreName = r.Order.Store.StoreName,
                                   r.ReviewId,
                                   ReviewRating = Convert.ToDouble(r.ReviewRating),
                                   r.ReviewContent,
                                   ReviewTime = r.ReviewTime.ToString("yyyy/MM/dd HH:mm")

                               };


            return View(queryComment.ToList());
        }
    }
}
