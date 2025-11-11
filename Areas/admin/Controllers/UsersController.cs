using Microsoft.AspNetCore.Mvc;
using _24dh111520_LTW.Models;
using Microsoft.EntityFrameworkCore;

namespace _24dh111520_LTW.Areas.admin.Controllers
{
    [Area("admin")]
    public class UsersController : Controller
    {
        private readonly MyStoreContext _context;
        public UsersController(MyStoreContext context) { _context = context; }

        // GET: Login form
        public IActionResult Login() => View();

        // POST: Xử lý đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                // Chuyển qua trang admin hoặc home
                return RedirectToAction("Index", "Dashboard", new { area = "admin" });
            }
            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
            return View();
        }

        // GET: Register form
        public IActionResult Register() => View();

        // POST: Xử lý đăng ký tài khoản mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                ViewBag.Error = "Tên tài khoản đã tồn tại";
                return View();
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            // Đăng ký xong chuyển qua đăng nhập
            return RedirectToAction("Login");
        }
    }
}


