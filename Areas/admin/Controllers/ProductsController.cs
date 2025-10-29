using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _24dh111520_LTW.Models;

namespace _24dh111520_LTW.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly MyStoreContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(MyStoreContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        //  INDEX - Tìm kiếm + sắp xếp + phân trang
        public async Task<IActionResult> Index(string search, string sortOrder, int page = 1)
        {
            int pageSize = 5;
            var products = _context.Products.Include(p => p.Category).AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p =>
                    EF.Functions.ILike(p.ProductName, $"%{search}%") ||
                    EF.Functions.ILike(p.ProductDescription, $"%{search}%") ||
                    EF.Functions.ILike(p.Category.CategoryName, $"%{search}%"));
            }

            // Sắp xếp
            ViewData["NameSort"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSort"] = sortOrder == "price" ? "price_desc" : "price";

            products = sortOrder switch
            {
                "name_desc" => products.OrderByDescending(p => p.ProductName),
                "price" => products.OrderBy(p => p.ProductPrice),
                "price_desc" => products.OrderByDescending(p => p.ProductPrice),
                _ => products.OrderBy(p => p.ProductName),
            };

            // Phân trang
            var totalItems = await products.CountAsync();
            var paged = await products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            Console.WriteLine("Số sản phẩm trả về view Index: " + paged.Count);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.Search = search;

            return View(paged);
        }

        //  CREATE (GET)
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.OrderBy(c => c.CategoryName).ToList();
            return View();
        }

        //  CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }


        //  EDIT (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        //  EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadFolder = Path.Combine(_env.WebRootPath, "images");
                    Directory.CreateDirectory(uploadFolder);

                    string filePath = Path.Combine(uploadFolder, ImageFile.FileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await ImageFile.CopyToAsync(stream);

                    product.ProductImage = "/images/" + ImageFile.FileName;
                }

                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        //  DELETE
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.Include(p => p.Category)
                                                 .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
