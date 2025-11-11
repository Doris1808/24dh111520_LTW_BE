using System;
namespace _24dh111520_LTW.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public string? ProductImage { get; set; }
        public int Quantity { get; set; } = 1;

        public decimal Total => ProductPrice * Quantity;
    }
}
