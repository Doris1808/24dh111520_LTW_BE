using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using _24dh111520_LTW.Models;

namespace _24dh111520_LTW.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // ✅ Khi mở trang chủ, tự chuyển đến /Admin/Categories
        return RedirectToAction("Index", "Categories", new { area = "Admin" });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
