using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;

namespace DecoranestBacknd.DecoraNest.Core.Entities
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public int? UserID { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
       

        [JsonIgnore]
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public DateTime AddedAt { get; set; }= DateTime.Now;
    }
}
