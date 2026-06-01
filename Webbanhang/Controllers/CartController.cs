using Microsoft.AspNetCore.Mvc;
using Webbanhang.Extensions;
using Webbanhang.Models;
using Webbanhang.Repositories;

namespace Webbanhang.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";
        private readonly IProductRepository _productRepository;

        public CartController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        public IActionResult AddToCart(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl
                });
            }
            else
            {
                item.Quantity++;
            }

            SaveCart(cart);
            TempData["Success"] = "Đã thêm sản phẩm vào giỏ hàng!";
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Increase(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                item.Quantity++;
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Decrease(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
                TempData["Success"] = "Đã xóa sản phẩm khỏi giỏ hàng!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["Error"] = "Giỏ hàng đang trống!";
                return RedirectToAction("Index");
            }

            HttpContext.Session.Remove(CartSessionKey);
            HttpContext.Session.SetInt32("CartCount", 0);
            TempData["Success"] = "Thanh toán thành công! Cảm ơn bạn đã mua hàng.";
            return RedirectToAction("Index", "Product");
        }

        private List<CartItem> GetCart()
        {
            return HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
            HttpContext.Session.SetInt32("CartCount", cart.Sum(x => x.Quantity));
        }
    }
}
