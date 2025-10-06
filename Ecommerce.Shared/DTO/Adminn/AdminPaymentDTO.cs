namespace DecoranestBacknd.Ecommerce.Shared.DTO.Adminn
{
    public class AdminPaymentDTO
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
