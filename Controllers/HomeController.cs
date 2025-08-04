using ASM_00357.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ASM_00357.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopDbContext _context;

        public HomeController(ShopDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString)
        {
            var products = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }

            return View(products.ToList());
        }
    }
}
