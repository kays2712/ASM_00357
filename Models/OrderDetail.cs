namespace ASM_00357.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; } // ✅ giá lúc mua, KHÔNG phải Price

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
