using ASM_00357.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASM_00357.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ShopDbContext _context;

        public CustomerController(ShopDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var list = _context.Customers.ToList();
            return View(list); // Không ghi tên view → mặc định dùng Index.cshtml
        }



        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id) => View(_context.Customers.Find(id));

        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            _context.Customers.Update(customer);
                _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var c = _context.Customers.Find(id);
            if (c != null)
            {
                _context.Customers.Remove(c);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
