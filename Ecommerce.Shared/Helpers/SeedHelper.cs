using DecoranestBacknd.DecoraNest.Core.Entities;
using DecoranestBacknd.Infrastructure.Data;

namespace DecoranestBacknd.Ecommerce.Shared.Helpers
{
    public class SeedHelper
    {
        public static void SeedProducts(ApplicationDbContext context) {

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        ProductName = "Wall Art Painting",
                        Description = "Modern abstract wall painting to enhance your living room.",
                        Category = "Wall Decor",
                        Price = 1499,
                        ImgUrl = "https://example.com/wallart.png"

                    } , 
                    new Product
                    {
                        ProductName = "Decorative Vase",
                        Description = "Elegant ceramic vase for flowers or decoration.",
                        Category = "Table Decor",
                        Price = 799,
                        ImgUrl = "https://example.com/decorativevase.png"
                    },
                    new Product
                    {
                        ProductName = "LED Table Lamp",
                        Description = "Stylish LED lamp for a cozy ambient lighting.",
                        Category = "Lighting",
                        Price = 1299,
                        ImgUrl = "https://example.com/ledlamp.png"
                    },
                    new Product
                    {
                        ProductName = "Indoor Plant",
                        Description = "Lush green indoor plant for a refreshing atmosphere.",
                        Category = "Plants",
                        Price = 599,
                        ImgUrl = "https://example.com/indoorplant.png"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
