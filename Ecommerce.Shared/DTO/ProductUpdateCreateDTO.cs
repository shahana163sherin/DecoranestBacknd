namespace DecoranestBacknd.Ecommerce.Shared.DTO
{
    public class ProductUpdateCreateDTO
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public IFormFile? ImageFile { get; set; }
        //public int Amount { get; set; }
    }
}
