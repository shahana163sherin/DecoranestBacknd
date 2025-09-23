using System.ComponentModel.DataAnnotations;

namespace DecoranestBacknd.DecoraNest.Core.Entities
{
    public class Wishlist
    {
        [Key]
        public int WishListID { get; set; }
        public int UserID { get; set; }
         public User? User { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.Now;
    }
}
