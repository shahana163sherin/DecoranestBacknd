using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DecoranestBacknd.DecoraNest.Core.Entities
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public int CartId { get; set; }

        [JsonIgnore]
        public Cart? Cart { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

       
        public DateTime AddedAt { get; set; } = DateTime.Now;
    }
}
