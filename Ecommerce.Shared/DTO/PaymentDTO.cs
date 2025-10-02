namespace DecoranestBacknd.Ecommerce.Shared.DTO
{
    public class PaymentDTO
    {
        public string RazorpayPaymentId { get; set; }
        public string RazorpayOrderId { get; set; }
        public string RazorpaySignature { get; set; }
    }

}
