namespace DecoranestBacknd.Ecommerce.Shared.DTO
{
    public class CartDto
    {
        public int CartId { get; set; }
        public int UserID { get; set; }
        public DateTime AddedAt { get; set; }
        public string Message { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }
}
