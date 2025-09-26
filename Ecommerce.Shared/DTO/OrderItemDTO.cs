namespace DecoranestBacknd.Ecommerce.Shared.DTO
{
    public class OrderItemDTO
    {
       public int ProductId { get; set; }
       public string  ProductName{get;set;}
       public decimal Price { get; set; }

        public int Quantity { get; set; }
        public string ImgUrl { get; set; }

    }
}
