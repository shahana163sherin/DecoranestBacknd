﻿using System.ComponentModel.DataAnnotations;

namespace DecoranestBacknd.DecoraNest.Core.Entities
{
    public class OrderItem
    {
        [Key]
        public int ItemID { get; set; }
        public int OrderID { get; set; }
        public Order? Order { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImgUrl { get; set; }

    }
}
