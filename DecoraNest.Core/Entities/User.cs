using System.ComponentModel.DataAnnotations;

namespace DecoranestBacknd.DecoraNest.Core.Entities
{
    public class User
    {
        [Key]
        public int User_Id { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsBlocked { get; set; } = false;
        //public bool isDeleted { get; set; } = false;
        public DateTime CreateAt { get; set; }= DateTime.Now;
        public ICollection<Order> Orders { get; set; }=new List<Order>();
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();



    }
}
