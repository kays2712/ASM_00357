using ASM_00357.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASM_00357.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopDbContext _context;
        public CartController(ShopDbContext context) => _context = context;


        public IActionResult Index()
        {
            int? customerId = HttpContext.Session.GetInt32("UserId");
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int id = customerId.Value;
            var cart = _context.Carts.FirstOrDefault(c => c.CustomerId == id);

            if (cart == null)
            {
                // Trả về view rỗng nếu chưa có giỏ
                ViewBag.CartDetails = new List<CartDetail>();
                ViewBag.Total = 0;
                return View(new List<CartDetail>());
            }

            var details = _context.CartDetails
                .Include(cd => cd.Product)
                .Where(cd => cd.CartId == cart.CartId)
                .ToList();

            ViewBag.Total = details.Sum(d => d.Quantity * d.Product.Price);  // ✅ đảm bảo không lỗi null

            return View(details);
        }



        public IActionResult AddToCart(int productId)
        {
            int? customerId = HttpContext.Session.GetInt32("UserId");
            if (customerId == null)
            {
                return RedirectToAction("Login", "Account"); // ⚠️ Nếu chưa đăng nhập
            }

            // Chuyển sang int sau khi chắc chắn khác null
            int id = customerId.Value;

            // Tìm giỏ hàng của user
            var cart = _context.Carts.FirstOrDefault(c => c.CustomerId == id);
            if (cart == null)
            {
                cart = new Cart { CustomerId = id };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            // Kiểm tra nếu đã có sản phẩm trong giỏ
            var item = _context.CartDetails.FirstOrDefault(cd => cd.CartId == cart.CartId && cd.ProductId == productId);
            if (item != null)
            {
                item.Quantity += 1;
            }
            else
            {
                item = new CartDetail
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = 1
                };
                _context.CartDetails.Add(item);
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        public IActionResult Remove(int id)
        {
            var item = _context.CartDetails.FirstOrDefault(cd => cd.CartDetailId == id);
            if (item != null)
            {
                _context.CartDetails.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var detail = _context.CartDetails.Find(id);
            if (detail != null && quantity > 0)
            {
                detail.Quantity = quantity;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Checkout()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var cart = _context.Carts.FirstOrDefault(c => c.CustomerId == userId);
            if (cart == null)
            {
                TempData["Message"] = "Không tìm thấy giỏ hàng.";
                return RedirectToAction("Index");
            }

            var cartItems = _context.CartDetails
                .Include(c => c.Product)
                .Where(c => c.CartId == cart.CartId)
                .ToList();

            if (cartItems == null || !cartItems.Any())
            {
                TempData["Message"] = "Không có sản phẩm trong giỏ!";
                return RedirectToAction("Index");
            }

            var order = new Order
            {
                CustomerId = userId.Value,
                OrderDate = DateTime.Now,
                TotalAmount = cartItems.Sum(i => i.Product.Price * i.Quantity),
                Address = "Địa chỉ mặc định" // 🚨 Tạm để tránh lỗi nếu bạn chưa thêm field này từ CSDL
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in cartItems)
            {
                _context.OrderDetails.Add(new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                });
            }

            _context.CartDetails.RemoveRange(cartItems);
            _context.Carts.Remove(cart);
            _context.SaveChanges();

            TempData["Message"] = "🛒 Đặt hàng thành công!";
            return RedirectToAction("Index");
        }




    }
}
