using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
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

        public ManagerProductsController(VitaContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {

            return View(GetProducts(1));
        }


        [HttpPost]
        public IActionResult Index(int currentPageIndex)
        {
            return View(GetProducts(currentPageIndex));

        }

        public ActionResult CategoryDetails()
        {
            var categories = GetProductCategories();
            return PartialView("CategoryDetails", categories);

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
            _context.ProductCategories.Remove(item);
            _context.SaveChanges();

            var categories = GetProductCategories();
            return PartialView("CategoryDetails", categories);
        }
        [HttpPost]
        public IActionResult CategoryCreate(string CategoryName)
        {
            ProductCategory item = new ProductCategory()
            {
                CategoryName = CategoryName,
                StoreId = 1,
                CategoryOnDelete = false,
            };
            _context.ProductCategories.Add(item);
            _context.SaveChanges();

            var categories = GetProductCategories();
            return PartialView("CategoryDetails", categories);
        }

        [HttpPost]
        public IActionResult ProductOnSellSwitch(int ProductId, bool ProductOnSell)
        {
            Product item = _context.Products.Find(ProductId);
            item.ProductOnSell = ProductOnSell;
            _context.SaveChanges();

            return Json(new { success = true });
        }
        public IActionResult ProductCopy(int id)
        {

            return View(GetSpecificProduct(id));
        }

        [HttpPost]
        public IActionResult ProductCopy(string ProductName, decimal ProductUnitPrice, short ProductUnitsInStock, string ProductOnSell, int CategoryId, IFormFile? ProductImage)
        {

            Product item = new Product()
            {
                ProductName = ProductName,
                ProductUnitPrice = ProductUnitPrice,
                ProductUnitsInStock = ProductUnitsInStock,
                ProductOnSell = Convert.ToBoolean(ProductOnSell),                    
                CategoryId = CategoryId,
                StoreId= 1
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


            return View("Index", GetProducts(1));


        }


        public IActionResult ProductEdit(int id)
        {
           

            return View(GetSpecificProduct(id));
        }


        [HttpPost]
        public IActionResult ProductEdit(ProductViewModel clientItem)
        {

            Product product = _context.Products.Find(clientItem.ProductId);
            if (product != null) { 

            product.ProductName = clientItem.ProductName;
            product.ProductUnitPrice = clientItem.ProductUnitPrice;
            product.ProductUnitsInStock = clientItem.ProductUnitsInStock;
            product.CategoryId = clientItem.CategoryId;

            _context.SaveChanges();

            }

            return Redirect("/ManagerProducts/Index");
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

            }

            _context.SaveChanges();



            return View("Index", GetProducts(1));
        }

        [HttpPost]
        public IActionResult PictureDelete(int ProductId)
        {
            var product = _context.Products.Find(ProductId);
            if (!string.IsNullOrEmpty(product.ProductImage)) {
                
                var fileName = product.ProductImage.Split('/').LastOrDefault();
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "Store", fileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

            }
            product.ProductImage = string.Empty;
            _context.SaveChanges();


            return Redirect("/ManagerProducts/Index");

        }
        private List<ProductCategory> GetProductCategories()
        {
            var queryProductCategories = from c in _context.ProductCategories
                                         where c.StoreId == 1 && c.CategoryOnDelete == false
                                         select new ProductCategory
                                         {
                                             CategoryId = c.CategoryId,
                                             CategoryName = c.CategoryName
                                         };
            var viewModel = queryProductCategories.ToList();

            return viewModel;

        }

        private ManagerProducts GetProducts(int currentPage)
        {
         
            int maxRows = 10; //每頁幾列'

            var queryProducts = from p in _context.Products
                                join c in _context.ProductCategories on p.CategoryId equals c.CategoryId
                                where p.StoreId == 1
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


            var products = queryProducts.Skip((currentPage - 1) * maxRows).Take(maxRows).ToList();

            var queryCategories = from c in _context.ProductCategories
                                  where c.StoreId == 1
                                  select new ProductCategory { 
                                     CategoryId = c.CategoryId,
                                     CategoryName = c.CategoryName,
                                     CategoryOnDelete = c.CategoryOnDelete,
                                     StoreId = 1
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
                                  where c.StoreId == 1
                                  select new ProductCategory
                                  {
                                      CategoryId = c.CategoryId,
                                      CategoryName = c.CategoryName,
                                      CategoryOnDelete = c.CategoryOnDelete,
                                      StoreId = 1
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

