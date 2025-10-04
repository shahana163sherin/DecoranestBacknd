namespace DecoranestBacknd.Ecommerce.Shared.DTO.Adminn
{
    public class AdminOrderDTO
    {
        public int OrdeId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }="Unpaid";
        public DateTime OrderDate { get; set; }
        public string Address { get; set; }
        public List<AdminOrderItemDTO> Items { get; set; } = new List<AdminOrderItemDTO>();
    }
}
