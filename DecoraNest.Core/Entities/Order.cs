using System.ComponentModel.DataAnnotations;

namespace DecoranestBacknd.DecoraNest.Core.Entities
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public User? User { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string Address { get; set; }
        public ICollection<OrderItem> Items { get; set; }=new List<OrderItem>();
    }
}
