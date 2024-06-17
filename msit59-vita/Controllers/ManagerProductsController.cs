using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Query.Internal;
using msit59_vita.Models;
using System.Collections;
using static msit59_vita.Models.ManagerProducts;

namespace msit59_vita.Controllers
{
    public class ManagerProductsController : Controller
    {
        private readonly VitaContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly int StoreId = 1;

        public ManagerProductsController(VitaContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // 首頁
        public IActionResult Index(string sortString)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                TempData["currentPageIndex"] = 1;
                return View(GetProducts(1, sortString));
            }
            else
            {
                return Redirect("/ManagerLogin");
            }
        }


        [HttpPost]
        public IActionResult Index(int currentPageIndex, string sortString)
        {
            TempData["currentPageIndex"] = currentPageIndex;

            return View(GetProducts(currentPageIndex, sortString));

        }

        [HttpPost]
        public IActionResult ProductOnSellSwitch(int ProductId, bool ProductOnSell)
        {
            Product item = _context.Products.Find(ProductId);
            ProductCategory category = _context.ProductCategories.Find(item.CategoryId);

            if (category.CategoryOnDelete == true && ProductOnSell == true)
            {
                return Json(new { success = false, message = "此類別已被刪除，如仍要上架商品請去「編輯」頁面修改類別" });
            }
            else
            {
                item.ProductOnSell = ProductOnSell;
                _context.SaveChanges();

                return Json(new { success = true, message = "修改上架狀態成功" });
            }


        }


        //商品分類彈窗
        public ActionResult CategoryDetails()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var categories = GetProductCategories();

                return PartialView("CategoryDetails", categories);
            }
            else
            {
                return Redirect("/ManagerLogin");
            }


        }
    



        public IActionResult CategoryEdit(int CategoryId, string CategoryName)
        {

            var categoryItem = _context.ProductCategories.Find(CategoryId);
            categoryItem.CategoryName = CategoryName;
            categoryItem.CategoryOnDelete = false;
            _context.SaveChanges();

            var categories = GetProductCategories();
            return PartialView("CategoryDetails", categories);
        }

        [HttpPost]
        public IActionResult CategoryDelete(int CategoryId)
        {
            ProductCategory item = _context.ProductCategories.Find(CategoryId);
            //_context.ProductCategories.Remove(item);

            var products = _context.Products.Where(p => p.CategoryId == CategoryId && p.StoreId == StoreId).ToList();

            if (products == null)
            {
                item.CategoryOnDelete = true;
                _context.SaveChanges();

                return Json(new { success = true, message = "刪除成功" });
            }

            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].ProductOnSell == true)
                {
                    return Json(new { success = false, message = "仍有上架商品屬於此類別，請刪掉後再操作" });
                }
            }

            item.CategoryOnDelete = true;
            _context.SaveChanges();

            return Json(new { success = true, message = "刪除成功" });
        }

        [HttpPost]
        public IActionResult CategoryCreate(string CategoryName)
        {
            ProductCategory item = new ProductCategory()
            {
                CategoryName = CategoryName,
                StoreId = StoreId,
                CategoryOnDelete = false,
            };
            _context.ProductCategories.Add(item);
            _context.SaveChanges();

            var categories = GetProductCategories();
            return PartialView("CategoryDetails", categories);
        }




        // ProductCopy頁面
        public IActionResult ProductCopy(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                 Product item = _context.Products.Find(id);
                if (item != null)
                {
                     int thisCategoryId = item.CategoryId;
                    var productname = from p in _context.Products
                                      where p.CategoryId == thisCategoryId 
                                      select p.ProductName;

                    var productnamelist = productname.Distinct().ToList();
                    ViewBag.ProductNameList = productnamelist;
                }
                                
                return View(GetSpecificProduct(id));
            }
            else
            {
                return Redirect("/ManagerLogin");
            }
        }


        [HttpPost]
        public IActionResult ProductCopy(string ProductName, int ProductUnitPrice, short ProductUnitsInStock, string ProductOnSell, int CategoryId, IFormFile? ProductImage)
        {

            Product item = new Product()
            {
                ProductName = ProductName,
                ProductUnitPrice = ProductUnitPrice,
                ProductUnitsInStock = ProductUnitsInStock,
                ProductOnSell = Convert.ToBoolean(ProductOnSell),
                CategoryId = CategoryId,
                StoreId = StoreId
            };
            _context.Products.Add(item);
            _context.SaveChanges();


            if (ProductImage != null)
            {
                var fileName = $"Product_{item.ProductId}_" + Path.GetFileName(ProductImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "Store", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ProductImage.CopyTo(stream);
                }

                item.ProductImage = "image/Store/" + fileName;
                _context.SaveChanges();
            }

            int currentPageIndex = TempData.ContainsKey("currentPageIndex") ? (int)TempData["currentPageIndex"] : 1;

            return View("Index", GetProducts(currentPageIndex, "id_asc"));


        }

        public IActionResult ProductCreate()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var categories = GetProductCategories();
                var productname = from p in _context.Products
                                  where p.StoreId == StoreId
                                  select p.ProductName;
                var productnamelist = productname.Distinct().ToList();
                ViewBag.ProductNameList = productnamelist;

                return View(categories);
            }
            else
            {
                return Redirect("/ManagerLogin");
            }

        }

        [HttpPost]
        public IActionResult ProductCreate(string ProductName, int ProductUnitPrice, short ProductUnitsInStock, int CategoryId, IFormFile? ProductImage)
        {
            Product item = new Product()
            {
                ProductName = ProductName,
                ProductUnitPrice = ProductUnitPrice,
                ProductUnitsInStock = ProductUnitsInStock,
                ProductOnSell = true,
                CategoryId = CategoryId,
                StoreId = StoreId
            };


            _context.Products.Add(item);
            _context.SaveChanges();

            // 若有上傳圖片，則儲存圖片檔案
            if (ProductImage != null)
            {
                var fileName = $"Product_{item.ProductId}_" + Path.GetFileName(ProductImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "Store", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ProductImage.CopyTo(stream);
                }

                item.ProductImage = "image/Store/" + fileName;
                _context.SaveChanges();
            }

            return View("Index", GetProducts(1, "id_asc"));


        }


        // ProductEdit頁面
        public IActionResult ProductEdit(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                Product item = _context.Products.Find(id);
                if (item != null)
                {
                    int thisCategoryId = item.CategoryId;
                    var productname = from p in _context.Products
                                      where p.CategoryId == thisCategoryId && p.ProductName != item.ProductName
                                      select p.ProductName;

                    var productnamelist = productname.Distinct().ToList();
                    ViewBag.ProductNameList = productnamelist;
                }


                    return View(GetSpecificProduct(id));
            }
            else
            {
                return Redirect("/ManagerLogin");
            }
        }

        [HttpPost]
        public IActionResult ProductEdit(int ProductId, string ProductName, int ProductUnitPrice, short ProductUnitsInStock, string ProductOnSell, int CategoryId)
        {

            Product product = _context.Products.Find(ProductId);
            if (product != null)
            {

                product.ProductName = ProductName;
                product.ProductUnitPrice = ProductUnitPrice;
                product.ProductUnitsInStock = ProductUnitsInStock;
                product.CategoryId = CategoryId;

                _context.SaveChanges();

            }

            int currentPageIndex = TempData.ContainsKey("currentPageIndex") ? (int)TempData["currentPageIndex"] : 1;

            //return Redirect("/ManagerProducts/Index"); 
            return View("Index", GetProducts(currentPageIndex, "id_asc"));
        }

        [HttpPost]
        public IActionResult PictureUpload(int ProductId, IFormFile ProductImage)
        {

            Product product = _context.Products.Find(ProductId);

            if (ProductImage != null)
            {
                var fileName = $"Product_{ProductId}_" + Path.GetFileName(ProductImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "Store", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ProductImage.CopyTo(stream);
                }

                product.ProductImage = "image/Store/" + fileName;
                _context.SaveChanges();

                return Json(new { success = true, message = "圖片成功上傳" });
            }
            else
            {
                return Json(new { success = false, message = "圖片無法上傳，請稍後再試" });
            }

        }

        [HttpPost]
        public IActionResult PictureDelete(int ProductId)
        {
            var product = _context.Products.Find(ProductId);
            if (!string.IsNullOrEmpty(product.ProductImage))
            {

                var fileName = product.ProductImage.Split('/').LastOrDefault();
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "Store", fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

            }

            product.ProductImage = string.Empty;
            _context.SaveChanges();


            return Json(new { success = true, message = "圖片成功刪除" });

        }


        //用到的函數

        private List<ProductCategory> GetProductCategories()
        {
            var queryProductCategories = from c in _context.ProductCategories
                                         where c.StoreId == StoreId && c.CategoryOnDelete == false
                                         select new ProductCategory
                                         {
                                             CategoryId = c.CategoryId,
                                             CategoryName = c.CategoryName
                                         };
            var viewModel = queryProductCategories.ToList();

            return viewModel;

        }

        private ManagerProducts GetProducts(int currentPage, string sortString)
        {

            int maxRows = 10; //每頁幾列'

            var queryProducts = from p in _context.Products
                                join c in _context.ProductCategories on p.CategoryId equals c.CategoryId
                                where p.StoreId == StoreId
                                orderby p.ProductOnSell descending, p.CategoryId ascending
                                select new ProductViewModel
                                {
                                    ProductId = p.ProductId,
                                    ProductImage = p.ProductImage,
                                    ProductName = p.ProductName,
                                    ProductUnitPrice = p.ProductUnitPrice,
                                    ProductUnitsInStock = p.ProductUnitsInStock,
                                    ProductOnSell = p.ProductOnSell,
                                    CategoryId = c.CategoryId,
                                    CategoryName = c.CategoryName
                                };

            switch (sortString)
            {
                case "id_asc":
                    queryProducts = queryProducts.OrderBy(p => p.ProductId);
                    break;
                case "id_desc":
                    queryProducts = queryProducts.OrderByDescending(p => p.ProductId);
                    break;
                case "Category_id_asc":
                    queryProducts = queryProducts.OrderBy(p => p.ProductName);
                    break;
                case "Category_id_desc":
                    queryProducts = queryProducts.OrderByDescending(p => p.ProductName);
                    break;
                case "price_asc":
                    queryProducts = queryProducts.OrderBy(p => p.ProductUnitPrice);
                    break;
                case "price_desc":
                    queryProducts = queryProducts.OrderByDescending(p => p.ProductUnitPrice);
                    break;
                default:

                    queryProducts = queryProducts.OrderBy(p => p.ProductId);
                    break;
            }


            var products = queryProducts.Skip((currentPage - 1) * maxRows).Take(maxRows).ToList();

            var queryCategories = from c in _context.ProductCategories
                                  where c.StoreId == StoreId && c.CategoryOnDelete == false
                                  select new ProductCategory
                                  {
                                      CategoryId = c.CategoryId,
                                      CategoryName = c.CategoryName,
                                      CategoryOnDelete = c.CategoryOnDelete,
                                      StoreId = StoreId
                                  };
            var categories = queryCategories.ToList();

            var viewModel = new ManagerProducts
            {
                Products = products,
                Categories = categories
            };

            double pageCount = (double)((decimal)queryProducts.ToList().Count() / Convert.ToDecimal(maxRows));
            viewModel.PageCount = (int)Math.Ceiling(pageCount); //總共頁數
            viewModel.CurrentPageIndex = currentPage; //目前所在分頁
            viewModel.MaxRows = maxRows;



            return viewModel;


        }
        private ManagerProducts GetSpecificProduct(int ProductId)
        {

            var queryProducts = from p in _context.Products
                                join c in _context.ProductCategories on p.CategoryId equals c.CategoryId
                                where p.ProductId == ProductId
                                orderby p.ProductOnSell descending, p.StoreId ascending
                                select new ProductViewModel
                                {
                                    ProductId = p.ProductId,
                                    ProductImage = p.ProductImage,
                                    ProductName = p.ProductName,
                                    ProductUnitPrice = p.ProductUnitPrice,
                                    ProductUnitsInStock = p.ProductUnitsInStock,
                                    ProductOnSell = p.ProductOnSell,
                                    CategoryId = c.CategoryId,
                                    CategoryName = c.CategoryName
                                };

            var products = queryProducts.ToList();

            var queryCategories = from c in _context.ProductCategories
                                  where c.StoreId == StoreId && c.CategoryOnDelete == false
                                  select new ProductCategory
                                  {
                                      CategoryId = c.CategoryId,
                                      CategoryName = c.CategoryName,
                                      CategoryOnDelete = c.CategoryOnDelete,
                                      StoreId = StoreId
                                  };
            var categories = queryCategories.ToList();

            var viewModel = new ManagerProducts
            {
                Products = products,
                Categories = categories
            };

            return viewModel;
        }
    }
    

}
