using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _24dh111520_LTW.Models;
using Microsoft.EntityFrameworkCore;

namespace _24dh111520_LTW.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AccountController : Controller
    {
        private readonly MyStoreContext _context;

        public AccountController(MyStoreContext context)
        {
            _context = context;
        }

        // GET: /Customer/Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Customer/Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại!");
                return View(user);
            }

            // Tạo tài khoản mới
            user.UserRole = 'U'; // 'U' = User, 'A' = Admin
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký thành công, hãy đăng nhập!";
            return RedirectToAction("Login");
        }

        // ✅ GET: /Customer/Account/Login
        public IActionResult Login(string? returnUrl = null)
        {
            // Lưu URL gốc để quay lại sau khi đăng nhập
            if (!string.IsNullOrEmpty(returnUrl))
                HttpContext.Session.SetString("ReturnUrl", returnUrl);

            return View();
        }

        // ✅ POST: /Customer/Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu!");
                return View();
            }

            // Lưu session
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("UserRole", user.UserRole.ToString());

            // Quay lại trang trước đó nếu có
            var returnUrl = HttpContext.Session.GetString("ReturnUrl");
            if (!string.IsNullOrEmpty(returnUrl))
            {
                HttpContext.Session.Remove("ReturnUrl");
                return Redirect(returnUrl);
            }

            // Mặc định quay lại trang chủ khách hàng
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        // ✅ GET: /Customer/Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
