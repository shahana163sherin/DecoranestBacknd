using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DecoranestBacknd.DecoraNest.Core.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public string RazorPayOrderId { get; set; }
        public int OrderId { get; set; }
        public string? RazorpayPaymentId { get; set; }
        public string? RazorPaySignature { get; set; }
        public decimal Amount { get; set; }
        //public string PaymentMethod { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime PaymentDate { get; set; }= DateTime.Now;
        [JsonIgnore]
        public Order Order { get; set; }


    }
}
