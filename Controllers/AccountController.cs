using ASM_00357.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ASM_00357.Controllers
{
    public class AccountController : Controller
    {
        private readonly ShopDbContext _context;

        public AccountController(ShopDbContext context)
        {
            _context = context;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Customers.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.CustomerId);
                HttpContext.Session.SetString("Role", user.Role.ToString());
                HttpContext.Session.SetString("Username", user.Username); // ✅ thêm dòng này

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = "Login failed";
            return View();
        }
        public IActionResult Profile()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var customer = _context.Customers.Find(userId);
            var orders = _context.CartDetails
                .Include(cd => cd.Product)
                .Include(cd => cd.Cart)
                .Where(cd => cd.Cart.CustomerId == userId)
                .ToList();

            var vm = new ProfileViewModel
            {
                Customer = customer,
                Orders = orders
            };

            return View(vm);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(Customer model)
        {
            model.Role = UserRole.Customer.ToString(); 

            _context.Customers.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }
    }
}