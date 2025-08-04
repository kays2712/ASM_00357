namespace ASM_00357.Models
{
    public class CartDetail
    {
        public int CartDetailId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
