using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _24dh111520_LTW.Models;
using Newtonsoft.Json;

namespace _24dh111520_LTW.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly MyStoreContext _context;
        public CartController(MyStoreContext context)
        {
            _context = context;
        }

        // 🔒 Hàm kiểm tra đăng nhập
        private bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("Username"));
        }

        // 🧠 Lấy giỏ hàng từ session
        private List<CartItem> GetCart()
        {
            var session = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(session))
                return new List<CartItem>();
            return JsonConvert.DeserializeObject<List<CartItem>>(session) ?? new List<CartItem>();
        }

        // 💾 Lưu giỏ hàng vào session
        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

        // 🛒 Hiển thị giỏ hàng
        public IActionResult Index()
        {
            // 🔐 Kiểm tra đăng nhập
            if (!IsLoggedIn())
            {
                var returnUrl = Url.Action("Index", "Cart", new { area = "Customer" });
                return RedirectToAction("Login", "Account", new { area = "Customer", returnUrl });
            }

            var cart = GetCart();
            return View(cart);
        }

        // ➕ Thêm sản phẩm vào giỏ
        public IActionResult AddToCart(int id)
        {
            // 🔐 Nếu chưa đăng nhập → chuyển sang trang login
            if (!IsLoggedIn())
            {
                var returnUrl = Url.Action("AddToCart", "Cart", new { area = "Customer", id });
                return RedirectToAction("Login", "Account", new { area = "Customer", returnUrl });
            }

            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            var cart = GetCart();
            var item = cart.FirstOrDefault(p => p.ProductId == id);

            if (item != null)
                item.Quantity++;
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductPrice = product.ProductPrice,
                    ProductImage = product.ProductImage,
                    Quantity = 1
                });
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // ❌ Xóa sản phẩm khỏi giỏ
        public IActionResult Remove(int id)
        {
            if (!IsLoggedIn())
            {
                var returnUrl = Url.Action("Index", "Cart", new { area = "Customer" });
                return RedirectToAction("Login", "Account", new { area = "Customer", returnUrl });
            }

            var cart = GetCart();
            var item = cart.FirstOrDefault(p => p.ProductId == id);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        // 🧹 Xóa toàn bộ giỏ hàng
        public IActionResult Clear()
        {
            if (!IsLoggedIn())
            {
                var returnUrl = Url.Action("Index", "Cart", new { area = "Customer" });
                return RedirectToAction("Login", "Account", new { area = "Customer", returnUrl });
            }

            SaveCart(new List<CartItem>());
            return RedirectToAction("Index");
        }
    }
}
