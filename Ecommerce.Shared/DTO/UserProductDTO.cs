namespace DecoranestBacknd.Ecommerce.Shared.DTO
{
    public class UserProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
