﻿namespace DecoranestBacknd.Ecommerce.Shared.DTO
{
    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public ProductDto Product { get; set; }
    }
}
