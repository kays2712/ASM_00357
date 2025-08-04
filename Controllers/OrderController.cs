using ASM_00357.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ASM_00357.Controllers
{
    public class OrderController : Controller
    {
        private readonly ShopDbContext _context;

        public OrderController(ShopDbContext context)
        {
            _context = context;
        }

        public IActionResult CreateOrder()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // xử lý tạo đơn hàng...
            TempData["Message"] = "Đặt hàng thành công!";
            return RedirectToAction("History");
        }

        public IActionResult History()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var orders = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.CustomerId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

    }
}
