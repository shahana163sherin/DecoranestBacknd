using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace DecoranestBacknd.DecoraNest.Core.Entities
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string ImgUrl { get; set; }
        public ICollection<Cart> Carts { get; set; }=new List<Cart>();
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

    }
}
