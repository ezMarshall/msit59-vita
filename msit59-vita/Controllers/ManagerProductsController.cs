using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using msit59_vita.Models;
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
        public async Task<IActionResult> Index(string sortString)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                TempData["currentPageIndex"] = 1;
                return View(await GetProductsAsync(1, sortString));
            }
            else
            {
                return Redirect("/ManagerLogin");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Index(int currentPageIndex, string sortString)
        {
            TempData["currentPageIndex"] = currentPageIndex;

            return View(await GetProductsAsync(currentPageIndex, sortString));

        }

        [HttpPost]
        public async Task<IActionResult> ProductOnSellSwitch(int ProductId, bool ProductOnSell)
        {
            Product item = await _context.Products.FindAsync(ProductId);
            ProductCategory category = await _context.ProductCategories.FindAsync(item.CategoryId);

            if (category.CategoryOnDelete == true && ProductOnSell == true)
            {
                return Json(new { success = false, message = "此類別已被刪除，如仍要上架商品請去「編輯」頁面修改類別" });
            }
            else
            {
                item.ProductOnSell = ProductOnSell;
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "修改上架狀態成功" });
            }


        }


        //商品分類彈窗
        public async Task<IActionResult> CategoryDetails()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var categories = await GetProductCategoriesAsync();

                return PartialView("CategoryDetails", categories);
            }
            else
            {
                return Redirect("/ManagerLogin");
            }


        }




        public async Task<IActionResult> CategoryEdit(int CategoryId, string CategoryName)
        {

            var categoryItem = await _context.ProductCategories.FindAsync(CategoryId);
            categoryItem.CategoryName = CategoryName;
            categoryItem.CategoryOnDelete = false;
            await _context.SaveChangesAsync();

            var categories = await GetProductCategoriesAsync();
            return PartialView("CategoryDetails", categories);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryDelete(int CategoryId)
        {
            ProductCategory item =await _context.ProductCategories.FindAsync(CategoryId);
            //_context.ProductCategories.Remove(item);

            var products = await _context.Products.Where(p => p.CategoryId == CategoryId && p.StoreId == StoreId).ToListAsync();

            if (products == null)
            {
                item.CategoryOnDelete = true;
                await _context.SaveChangesAsync();

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
            _context.SaveChangesAsync();

            return Json(new { success = true, message = "刪除成功" });
        }

        [HttpPost]
        public async Task<IActionResult> CategoryCreate(string CategoryName)
        {
            ProductCategory item = new ProductCategory()
            {
                CategoryName = CategoryName,
                StoreId = StoreId,
                CategoryOnDelete = false,
            };
            _context.ProductCategories.Add(item);
            await _context.SaveChangesAsync();

            var categories =await GetProductCategoriesAsync();
            return PartialView("CategoryDetails", categories);
        }




        // ProductCopy頁面
        public async Task<IActionResult> ProductCopy(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                Product item = await _context.Products.FindAsync(id);
                if (item != null)
                {
                    int thisCategoryId = item.CategoryId;
                    var productname = from p in _context.Products
                                      where p.CategoryId == thisCategoryId
                                      select p.ProductName;

                    var productnamelist = await productname.Distinct().ToListAsync();
                    ViewBag.ProductNameList = productnamelist;
                }

                return View(await GetSpecificProductAsync(id));
            }
            else
            {
                return Redirect("/ManagerLogin");
            }
        }


        [HttpPost]
        public async Task<IActionResult> ProductCopy(string ProductName, int ProductUnitPrice, short ProductUnitsInStock, string ProductOnSell, int CategoryId, IFormFile? ProductImage)
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
            await _context.SaveChangesAsync();


            if (ProductImage != null)
            {
                var fileName = $"Product_{item.ProductId}_" + Path.GetFileName(ProductImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "Store", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProductImage.CopyToAsync(stream);
                }

                item.ProductImage = "image/Store/" + fileName;
               await _context.SaveChangesAsync();
            }

            int currentPageIndex = TempData.ContainsKey("currentPageIndex") ? (int)TempData["currentPageIndex"] : 1;

            return View("Index", await GetProductsAsync(currentPageIndex, "id_asc"));


        }

        public async Task<IActionResult> ProductCreate()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var categories = await GetProductCategoriesAsync();
                var productname = from p in _context.Products
                                  where p.StoreId == StoreId
                                  select p.ProductName;
                var productnamelist = await productname.Distinct().ToListAsync();
                ViewBag.ProductNameList = productnamelist;

                return View(categories);
            }
            else
            {
                return Redirect("/ManagerLogin");
            }

        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(string ProductName, int ProductUnitPrice, short ProductUnitsInStock, int CategoryId, IFormFile? ProductImage)
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
            await _context.SaveChangesAsync();

            // 若有上傳圖片，則儲存圖片檔案
            if (ProductImage != null)
            {
                var fileName = $"Product_{item.ProductId}_" + Path.GetFileName(ProductImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "Store", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProductImage.CopyToAsync(stream);
                }

                item.ProductImage = "image/Store/" + fileName;
                await _context.SaveChangesAsync();
            }

            return View("Index",await GetProductsAsync(1, "id_asc"));


        }


        // ProductEdit頁面
        public async Task<IActionResult> ProductEdit(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                Product item =await  _context.Products.FindAsync(id);
                if (item != null)
                {
                    int thisCategoryId = item.CategoryId;
                    var productname = from p in _context.Products
                                      where p.CategoryId == thisCategoryId && p.ProductName != item.ProductName
                                      select p.ProductName;

                    var productnamelist =await productname.Distinct().ToListAsync();
                    ViewBag.ProductNameList = productnamelist;
                }


                return View(await GetSpecificProductAsync(id));
            }
            else
            {
                return Redirect("/ManagerLogin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(int ProductId, string ProductName, int ProductUnitPrice, short ProductUnitsInStock, string ProductOnSell, int CategoryId)
        {

            Product product = await _context.Products.FindAsync(ProductId);
            if (product != null)
            {

                product.ProductName = ProductName;
                product.ProductUnitPrice = ProductUnitPrice;
                product.ProductUnitsInStock = ProductUnitsInStock;
                product.CategoryId = CategoryId;

               await _context.SaveChangesAsync();

            }

            int currentPageIndex = TempData.ContainsKey("currentPageIndex") ? (int)TempData["currentPageIndex"] : 1;

            //return Redirect("/ManagerProducts/Index"); 
            return View("Index", await GetProductsAsync(currentPageIndex, "id_asc"));
        }

        [HttpPost]
        public async Task<IActionResult> PictureUpload(int ProductId, IFormFile ProductImage)
        {

            Product product = await _context.Products.FindAsync(ProductId);

            if (ProductImage != null)
            {
                var fileName = $"Product_{ProductId}_" + Path.GetFileName(ProductImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "Store", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                   await ProductImage.CopyToAsync(stream);
                }

                product.ProductImage = "image/Store/" + fileName;
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "圖片成功上傳" });
            }
            else
            {
                return Json(new { success = false, message = "圖片無法上傳，請稍後再試" });
            }

        }

        [HttpPost]
        public async Task<IActionResult> PictureDelete(int ProductId)
        {
            var product = await _context.Products.FindAsync(ProductId);
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
            await _context.SaveChangesAsync();


            return Json(new { success = true, message = "圖片成功刪除" });

        }


        //用到的函數

        private async Task<List<ProductCategory>> GetProductCategoriesAsync()
        {
            var queryProductCategories = from c in _context.ProductCategories
                                         where c.StoreId == StoreId && c.CategoryOnDelete == false
                                         select new ProductCategory
                                         {
                                             CategoryId = c.CategoryId,
                                             CategoryName = c.CategoryName
                                         };
            var viewModel = await queryProductCategories.ToListAsync();

            return viewModel;

        }

        private async Task<ManagerProducts> GetProductsAsync(int currentPage, string sortString)
        {

            ViewBag.ProductIdSortParm = sortString == "id_desc" ? "id_asc" : "id_desc";
            ViewBag.CategoryIdParm = sortString == "category_id_desc" ? "category_id_asc" : "category_id_desc";
            ViewBag.ProductOnSellParm = sortString == "product_on_sell_asc" ? "product_on_sell_desc" : "product_on_sell_asc";
            ViewBag.ProductUnitsInStockParm = sortString == "product_units_in_stock_asc" ? "product_units_in_stock_desc" : "product_units_in_stock_asc";


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
                case "id_desc":
                    queryProducts = queryProducts.OrderByDescending(p => p.ProductId);
                    break;
                case "id_asc":
                    queryProducts = queryProducts.OrderBy(p => p.ProductId);
                    break;
                case "category_id_asc":
                    queryProducts = queryProducts.OrderBy(c => c.CategoryId);
                    break;
                case "category_id_desc":
                    queryProducts = queryProducts.OrderByDescending(c => c.CategoryId);
                    break;
                case "product_units_in_stock_asc":
                    queryProducts = queryProducts.OrderBy(p => p.ProductUnitsInStock);
                    break;
                case "product_units_in_stock_desc":
                    queryProducts = queryProducts.OrderByDescending(p => p.ProductUnitsInStock);
                    break;
                case "product_on_sell_asc":
                    queryProducts = queryProducts.OrderBy(p => p.ProductOnSell);
                    break;
                case "product_on_sell_desc":
                    queryProducts = queryProducts.OrderByDescending(p => p.ProductOnSell);
                    break;
                default:
                    queryProducts = queryProducts.OrderBy(p => p.ProductId);
                    break;
            }


            var products = await queryProducts.Skip((currentPage - 1) * maxRows).Take(maxRows).ToListAsync();

            var queryCategories = from c in _context.ProductCategories
                                  where c.StoreId == StoreId && c.CategoryOnDelete == false
                                  select new ProductCategory
                                  {
                                      CategoryId = c.CategoryId,
                                      CategoryName = c.CategoryName,
                                      CategoryOnDelete = c.CategoryOnDelete,
                                      StoreId = StoreId
                                  };
            var categories = await queryCategories.ToListAsync();

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
        private async Task<ManagerProducts> GetSpecificProductAsync(int ProductId)
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

            var products = await queryProducts.ToListAsync();

            var queryCategories = from c in _context.ProductCategories
                                  where c.StoreId == StoreId && c.CategoryOnDelete == false
                                  select new ProductCategory
                                  {
                                      CategoryId = c.CategoryId,
                                      CategoryName = c.CategoryName,
                                      CategoryOnDelete = c.CategoryOnDelete,
                                      StoreId = StoreId
                                  };
            var categories = await queryCategories.ToListAsync();

            var viewModel = new ManagerProducts
            {
                Products = products,
                Categories = categories
            };

            return viewModel;
        }
    }


}
