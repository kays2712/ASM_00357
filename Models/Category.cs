namespace ASM_00357.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        // Danh sách sản phẩm thuộc danh mục
        public ICollection<Product> Products { get; set; }
    }
}
