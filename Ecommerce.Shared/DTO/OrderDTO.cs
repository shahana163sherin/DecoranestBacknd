namespace DecoranestBacknd.Ecommerce.Shared.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Address { get; set; }
        public ICollection<OrderItemDTO> Items { get; set; }
    }
}
