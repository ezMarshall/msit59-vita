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
			//沒有登入購物車資料透過Session取得
			if (false)
			{
				//未登入，請先登入
				//判斷是否營業中，如果商品不在營業中，則不能加入購物車
				//未登入他有加入商品，之後登入的購物車資料會自動顯示，原先購物車資料和登入後購物車資料分，加入到登入後購物車裡面
				//要進行比較如果未登入購物車中有相同的商品
				//如果有相同商品，登入後購物車的數量會覆蓋原先購物車的數量，不會重複加入

				Console.WriteLine("Get cart from session");
            }
			else
			{
				//目前CustomerId固定的
				// 取得目前登入的使用者，顯示他的購物車內容
				var cart = from s in _context.ShoppingCarts
                           join c in _context.Customers on s.CustomerId equals c.CustomerId
                           join p in _context.Products on s.ProductId equals p.ProductId
						   join t in _context.Stores on p.StoreId equals t.StoreId
                           where s.CustomerId == 1
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
				return Json(cartList);                
            }
			
			
            
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
			if (false)
			{
				return Json(new { success = false, message = "請先登入" });
			}

			////取得使用者的購物車
		 //  var shoppingCart = _context.ShoppingCarts
			//   .FirstOrDefault(sc => sc.UserId == User.Identity.Name && sc.StoreId == storeId);

			
			//	// 檢查購物車中是否已有相同商品
			//	var cartItem = shoppingCart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
			//	if (cartItem != null)
			//	{
			//		// 如果有相同商品,更新數量
			//		cartItem.Quantity += quantity;
			//	}
			//	else
			//	{
			//		// 如果沒有相同商品,新增商品到購物車
			//		shoppingCart.CartItems.Add(new CartItem
			//		{
			//			ProductId = productId,
			//			Quantity = quantity
			//		});
			//	}
			

			_context.SaveChanges();

			return Json(new { success = true, message = "商品已加入購物車" });
		}

		[HttpPost]
		public IActionResult UpdateQuantity()
		{
			return Content("UpdateQuantity");
		}

		[HttpPost]
		public ActionResult Delete()
		{
			return  Content("Delete456465");
		}

	}
}
