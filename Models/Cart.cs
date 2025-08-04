namespace ASM_00357.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedDate { get; set; }

        public Customer Customer { get; set; }
        public ICollection<CartDetail> CartDetails { get; set; }
    }
}
