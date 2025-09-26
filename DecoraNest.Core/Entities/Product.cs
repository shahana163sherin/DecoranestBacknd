using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public ICollection<CartItem> CartItems { get; set; }=new List<CartItem>();
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

    }
}
