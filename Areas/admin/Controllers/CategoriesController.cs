using Microsoft.AspNetCore.Mvc;
using _24dh111520_LTW.Models;

namespace _24dh111520_LTW.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly MyStoreContext _context;

        public CategoriesController(MyStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }
    }
}
