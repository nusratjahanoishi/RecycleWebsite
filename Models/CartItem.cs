﻿namespace NextUses.Models
{
    public class CartItem
    {



        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public byte[]? Image { get; set; }

    }
}
