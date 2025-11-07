using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _24dh111520_LTW.Models;

namespace _24dh111520_LTW.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly MyStoreContext _context;
        public HomeController(MyStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        public IActionResult ProductDetail(int id)
        {
            var product = _context.Products
                                  .Include(p => p.Category)
                                  .FirstOrDefault(p => p.ProductId == id);
            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}
