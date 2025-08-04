namespace ASM_00357.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string Role { get; set; } = "User";

        public string Email { get; set; }       // ✅ Email
        public string Address { get; set; }     // ✅ Địa chỉ nhà

    }

}
