using System;
using System.Collections.Generic;

namespace _24dh111520_LTW.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductDecription { get; set; } = null!;

    public decimal ProductPrice { get; set; }

    public string? ProductImage { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
