using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace DecoranestBacknd.DecoraNest.Core.Entities
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public int UserID { get; set; }
        public User? User { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime AddedAt { get; set; }= DateTime.Now;
    }
}
