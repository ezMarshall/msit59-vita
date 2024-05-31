using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using msit59_vita.Models;
using System.Globalization;
using System.Linq;
using static msit59_vita.Models.Store;
using static System.Net.Mime.MediaTypeNames;

namespace msit59_vita.Controllers
{
    //  後台 店家資訊頁面
    public class ManagerStoresController : Controller
    {
        private readonly VitaContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ManagerStoresController(VitaContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var queryStore = from s in _context.Stores
                             where s.StoreId == 1
                             select s;
            var store = queryStore.FirstOrDefault();
            var queryStoreOpeningHours = from o in _context.StoreOpeningHours
                                         where o.StoreId == 1
                                         select new StoreOpeningHour
                                         {
                                             MyWeekDay = o.MyWeekDay,
                                             StoreOpenOrNot = o.StoreOpenOrNot,
                                             StoreOpeningTime = o.StoreOpeningTime,
                                             StoreClosingTime = o.StoreClosingTime
                                         };


            ViewBag.StoreOpeningHours = queryStoreOpeningHours.ToList();

            return View(store);
        }

        public ActionResult StoreInfoEdit()
        {
            var queryStore = from s in _context.Stores
                             where s.StoreId == 1
                             select s;
            var store = queryStore.FirstOrDefault();

            var queryStoreOpeningHours = from o in _context.StoreOpeningHours
                             where o.StoreId == 1
                             select new StoreOpeningHour
                             {
                                 MyWeekDay = o.MyWeekDay,
                                 StoreOpenOrNot = o.StoreOpenOrNot,
                                 StoreOpeningTime = o.StoreOpeningTime,
                                 StoreClosingTime = o.StoreClosingTime
                             };



            ViewBag.StoreOpeningHours = queryStoreOpeningHours.ToList();



            return PartialView("StoreInfoEdit", store);

        }

        [HttpPost]
        public ActionResult StoreInfoEdit(Store clientStore, List<StoreOpeningHour> clientOpeningHours, IFormFile? clientStoreImage)
        {

            var store = _context.Stores.Find(clientStore.StoreId);
            if (store != null) {
            store.StorePhoneNumber = clientStore.StorePhoneNumber; //required
            store.StoreUniformInvoiceVia = clientStore.StoreUniformInvoiceVia;
            store.StoreLinePay = clientStore.StoreLinePay;


                if (clientStoreImage != null)
                {
                    var fileName = $"Store_{store.StoreId}_" + Path.GetFileName(clientStoreImage.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "Store", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        clientStoreImage.CopyTo(stream);
                    }

                    string oldStoreImage = store.StoreImage;
                    if (oldStoreImage!= null)
                    {
                        string oldFileName = Path.GetFileName(oldStoreImage);
                        string oldFilePath = Path.Combine(Directory.GetCurrentDirectory(),  "wwwroot", "Image", "Store", oldFileName);
                        Console.WriteLine(oldFilePath);

                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath); // 删除旧的商铺图片文件
                        }

                    }
                    
                    store.StoreImage = "image/Store/" + fileName;



                }
            }

            var storeOpeningHoursList = _context.StoreOpeningHours.Where(o => o.StoreId == clientStore.StoreId).ToList();
            string[] DayOfWeek = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

            for (int i = 0; i < DayOfWeek.Length; i++)
            {

            var storeOpeningHour = _context.StoreOpeningHours
                    .SingleOrDefault(o => o.StoreId == clientStore.StoreId && o.MyWeekDay == DayOfWeek[i]);

            storeOpeningHour.StoreOpenOrNot = clientOpeningHours[i].StoreOpenOrNot == true ? true : false;
            storeOpeningHour.StoreOpeningTime = clientOpeningHours[i].StoreOpeningTime;
            storeOpeningHour.StoreClosingTime = clientOpeningHours[i].StoreClosingTime;

            }

            _context.SaveChanges();


            return Redirect("/ManagerStores/Index");
        }

    }
}
