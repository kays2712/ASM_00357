namespace ASM_00357.Models
{
    public class Order
    {

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }

        public string Address { get; set; }             // ✅ địa chỉ giao hàng
        public decimal TotalAmount { get; set; }        // ✅ tổng tiền đơn hàng

        public Customer Customer { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
