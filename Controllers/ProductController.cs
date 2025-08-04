using ASM_00357.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASM_00357.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShopDbContext _context;
        public ProductController(ShopDbContext context) => _context = context;

        public IActionResult Index() => View(_context.Products.Include(p => p.Category).ToList());

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();

            // Lấy danh sách ảnh trong wwwroot/images
            var images = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"))
                                  .Select(Path.GetFileName)
                                  .ToList();

            ViewBag.Images = images;

            return View();
        }
        [HttpPost]
        public IActionResult ConfirmOrder(int productId, string fullName, string address, decimal price)
        {
            // Ở đây bạn có thể lưu vào bảng Order nếu có

            TempData["OrderMessage"] = $"✅ Đặt hàng thành công cho {fullName}. Sẽ giao đến: {address}";
            return RedirectToAction("Details", new { id = productId });
        }


        [HttpPost]
        public IActionResult Create(Product p)
        {
            _context.Products.Add(p);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == id);
            if (product == null) return NotFound();
            return View(product);
        }
        

        public IActionResult Edit(int id)
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View(_context.Products.Find(id));
        }

        [HttpPost]
        public IActionResult Edit(Product p)
        {
            _context.Products.Update(p);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var p = _context.Products.Find(id);
            if (p != null) { _context.Products.Remove(p); _context.SaveChanges(); }
            return RedirectToAction("Index");
        }
    }
}