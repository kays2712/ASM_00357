using ASM_00357.Models;
using Microsoft.EntityFrameworkCore;

namespace ASM_00357.Data
{
    public static class SeedData
    {
        public static void Initialize(ShopDbContext context)
        {
            context.Database.Migrate();

            if (!context.Customers.Any())
            {
                context.Customers.AddRange(
                    new Customer { FullName = "Admin", Username = "admin", Password = "123", Role = UserRole.Admin.ToString() },
                    new Customer { FullName = "User1", Username = "user1", Password = "123", Role = UserRole.User.ToString() },
                    new Customer { FullName = "User2", Username = "user2", Password = "123", Role = UserRole.User.ToString() }
                );


                context.SaveChanges();
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "iPhone" },
                    new Category { Name = "Samsung" }
                );
                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                var cat1 = context.Categories.First();
                var cat2 = context.Categories.Skip(1).First();

                for (int i = 1; i <= 15; i++)
                {
                    context.Products.Add(new Product
                    {
                        Name = "Phone " + i,
                        Description = "Mô tả sản phẩm " + i,
                        Price = 1000 + i * 10,
                        CategoryId = (i % 2 == 0) ? cat1.CategoryId : cat2.CategoryId
                    });
                }
                context.SaveChanges();
            }
        }
    }
}
