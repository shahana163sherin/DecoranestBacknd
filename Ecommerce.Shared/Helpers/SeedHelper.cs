using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.Infrastructure.Data;
using System.Linq;

namespace DecoranestBacknd.Ecommerce.Shared.Helpers
{
    public class SeedHelper
    {
        public static void SeedProducts(ApplicationDbContext context)
        {
            // Seed categories first
            if (!context.Category.Any())
            {
                context.Category.AddRange(
                    new Category { CategoryName = "Wall Decor" },
                    new Category { CategoryName = "Table Decor" },
                    new Category { CategoryName = "Lighting" },
                    new Category { CategoryName = "Plants" }
                );
                context.SaveChanges();
            }

            // Get actual category IDs
            var categories = context.Category.ToList();

            // Seed products
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        ProductName = "Wall Art Painting",
                        Description = "Modern abstract wall painting to enhance your living room.",
                        CategoryId = categories.First(c => c.CategoryName == "Wall Decor").CategoryId,
                        Price = 1499,
                        ImgUrl = "https://example.com/wallart.png"
                    },
                    new Product
                    {
                        ProductName = "Decorative Vase",
                        Description = "Elegant ceramic vase for flowers or decoration.",
                        CategoryId = categories.First(c => c.CategoryName == "Table Decor").CategoryId,
                        Price = 799,
                        ImgUrl = "https://example.com/decorativevase.png"
                    },
                    new Product
                    {
                        ProductName = "LED Table Lamp",
                        Description = "Stylish LED lamp for a cozy ambient lighting.",
                        CategoryId = categories.First(c => c.CategoryName == "Lighting").CategoryId,
                        Price = 1299,
                        ImgUrl = "https://example.com/ledlamp.png"
                    },
                    new Product
                    {
                        ProductName = "Indoor Plant",
                        Description = "Lush green indoor plant for a refreshing atmosphere.",
                        CategoryId = categories.First(c => c.CategoryName == "Plants").CategoryId,
                        Price = 599,
                        ImgUrl = "https://example.com/indoorplant.png"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
