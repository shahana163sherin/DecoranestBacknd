namespace DecoranestBacknd.Ecommerce.Shared.DTO
{
    public class WishListDTO
    {
        public int WishlistId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImgUrl { get; set; }
        public decimal Price { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
