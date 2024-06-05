using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using msit59_vita.Models;
using System.Text;

namespace msit59_vita.Controllers
{
	public class ShoppingCartsController : Controller
	{
		private readonly VitaContext _context;

		public ShoppingCartsController(VitaContext context)
		{
			_context = context;
		}
		// GET: ShoppingCartsController
		public IActionResult Index()
		{
			//沒有登入購物車資料
			// 檢查使用者是否已登入

			if (!User.Identity?.IsAuthenticated ?? false)
			{
				//未登入狀態
				return Json(new { success = false, message = "未登入購物車狀態" });
			}


			var customer = from c in _context.Customers
						   where c.CustomerEmail == User.Identity.Name
						   select c;

			var customerId = customer.FirstOrDefault();
			var customerID = customerId.CustomerId;/* 獲取當前用戶的CustomerID */

				// 取得目前登入的使用者，顯示他的購物車內容
				var cart = from s in _context.ShoppingCarts
                           join c in _context.Customers on s.CustomerId equals c.CustomerId
                           join p in _context.Products on s.ProductId equals p.ProductId
						   join t in _context.Stores on p.StoreId equals t.StoreId
					   where s.CustomerId == customerID
                           select new
                           {
							   ShoppingCartId = s.ShoppingCartId,
                               CustomerID = s.CustomerId,
                               ProductId = s.ProductId,
                               Quantity = s.ShoppingCartQuantity,
						   StoreID = p.StoreId,
							   StoreName = t.StoreName,
                               ProductName = p.ProductName,
							   ProductUnitsInStock = p.ProductUnitsInStock,
							   Price = p.ProductUnitPrice,
                               ProductImage = p.ProductImage
                           };

				var cartList = cart.ToList();

			//return Json(cartList);
			return Json(new { success = true, message = "已登入購物車狀態", cart = cartList.ToList() });

			            
        }


		[HttpPost]
		public IActionResult AddToCart(int productId, int quantity, int storeId)
		{
			//加入商品到購物車
			//判斷是否登入，如果登入，則把購物車資料存入資料庫
			//判斷是否有相同商品，如果有相同商品，會覆蓋原先購物車的數量，否則新增商品到購物車



			//加入商品時要判斷購物車商品是否為同一家，用戶一次因為只能購買同一家店商品做結帳




			// 檢查使用者是否已登入
			//!User.Identity.IsAuthenticated
           
			if (!User.Identity?.IsAuthenticated ?? false)
			{
				Console.WriteLine("未登入sdfsdfsdffg");
				return Json(new { success = false, message = "請先登入" });
			}

			var customer = from c in _context.Customers
						   where c.CustomerEmail == User.Identity.Name
						   select c;

			var customerId = customer.FirstOrDefault();
			var customerID = customerId.CustomerId;/* 獲取當前用戶的CustomerID */

			
			//取得使用者的購物車
			var cart = from s in _context.ShoppingCarts
					   join c in _context.Customers on s.CustomerId equals c.CustomerId
					   join p in _context.Products on s.ProductId equals p.ProductId
					   join t in _context.Stores on p.StoreId equals t.StoreId
					   where s.CustomerId == customerID
					   select new
					   {
						   ShoppingCartId = s.ShoppingCartId,
						   CustomerID = s.CustomerId,
						   ProductId = s.ProductId,
						   Quantity = s.ShoppingCartQuantity,
						   StoreName = t.StoreName,
						   ProductName = p.ProductName,
						   ProductUnitsInStock = p.ProductUnitsInStock,
						   Price = p.ProductUnitPrice,
						   ProductImage = p.ProductImage
					   };
			var cartList = cart.ToList();

			// 檢查購物車中是否已有相同商品
			var cartItem = cartList.FirstOrDefault(ci => ci.ProductId == productId);
			if (cartItem != null)
			{
				// 如果有相同商品，更新數量
				var shoppingCart = _context.ShoppingCarts.FirstOrDefault(sc => sc.ShoppingCartId == cartItem.ShoppingCartId);
				if (shoppingCart != null)
				{
					shoppingCart.ShoppingCartQuantity += (short)quantity;
					if (shoppingCart.ShoppingCartQuantity >= 30)
					{
						shoppingCart.ShoppingCartQuantity = 30;
					}
					_context.SaveChanges();

				}
			}
			else
			{
				// 如果沒有相同商品,新增商品到購物車
				var newCartItem = new ShoppingCart
				{
					CustomerId = customerID,//CustomerId = customerId,
					ProductId = productId,
					ShoppingCartQuantity = (short)quantity
				};
				_context.ShoppingCarts.Add(newCartItem);
				_context.SaveChanges();

			}

			// 獲取更新後的購物車列表
			var updatedCart = from s in _context.ShoppingCarts
							  join c in _context.Customers on s.CustomerId equals c.CustomerId
							  join p in _context.Products on s.ProductId equals p.ProductId
							  join t in _context.Stores on p.StoreId equals t.StoreId
							  where s.CustomerId == customerID
							  select new
							  {
								  ShoppingCartId = s.ShoppingCartId,
								  CustomerID = s.CustomerId,
								  ProductId = s.ProductId,
								  Quantity = s.ShoppingCartQuantity,
								  StoreName = t.StoreName,
								  ProductName = p.ProductName,
								  ProductUnitsInStock = p.ProductUnitsInStock,
								  Price = p.ProductUnitPrice,
								  ProductImage = p.ProductImage
							  };

			return Json(new { success = true, message = "商品已加入購物車", cart = updatedCart.ToList() });
		}

		[HttpPost]
		public IActionResult AddToCartInput(int productId, int quantity, int storeId)
		{
			// 檢查使用者是否已登入
			//!User.Identity.IsAuthenticated
			if (!User.Identity?.IsAuthenticated ?? false)
			{
				return Json(new { success = false, message = "未登入" });
			}

			var customer = from c in _context.Customers
						   where c.CustomerEmail == User.Identity.Name
						   select c;

			var customerId = customer.FirstOrDefault();
			var customerID = customerId.CustomerId;/* 獲取當前用戶的CustomerID */

			//取得使用者的購物車
			var cart = from s in _context.ShoppingCarts
					   join c in _context.Customers on s.CustomerId equals c.CustomerId
					   join p in _context.Products on s.ProductId equals p.ProductId
					   join t in _context.Stores on p.StoreId equals t.StoreId
					   where s.CustomerId == customerID
					   select new
					   {
						   ShoppingCartId = s.ShoppingCartId,
						   CustomerID = s.CustomerId,
						   ProductId = s.ProductId,
						   Quantity = s.ShoppingCartQuantity,
						   StoreName = t.StoreName,
						   ProductName = p.ProductName,
						   ProductUnitsInStock = p.ProductUnitsInStock,
						   Price = p.ProductUnitPrice,
						   ProductImage = p.ProductImage
					   };
			var cartList = cart.ToList();

			// 檢查購物車中是否已有相同商品
			var cartItem = cartList.FirstOrDefault(ci => ci.ProductId == productId);
			if (cartItem != null)
			{
				// 如果有相同商品，更新數量
				var shoppingCart = _context.ShoppingCarts.FirstOrDefault(sc => sc.ShoppingCartId == cartItem.ShoppingCartId);
				if (shoppingCart != null)
				{
					shoppingCart.ShoppingCartQuantity = (short)quantity;
					if (shoppingCart.ShoppingCartQuantity >= 30)
					{
						shoppingCart.ShoppingCartQuantity = 30;
					}            
                    _context.SaveChanges();

				}
			}

			return Json(new { success = true, message = "商品已加入購物車" });
		}

			[HttpPost]
		public IActionResult Delete(int id)
		{
			var cartItem = _context.ShoppingCarts.FirstOrDefault(sc => sc.ShoppingCartId == id);
			if (cartItem != null)
			{
				_context.ShoppingCarts.Remove(cartItem);
				_context.SaveChanges();
				return Json(new { success = true, message = "Item deleted successfully." });
			}
			return Json(new { success = false, message = "Item not found." });
		}

	}
}
