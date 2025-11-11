using System;
using System.Collections.Generic;

namespace _24dh111520_LTW.Models;

public partial class Product
{
    public int Productid { get; set; }

    public int Categoryid { get; set; }

    public string Productname { get; set; } = null!;

    public string Productdecription { get; set; } = null!;

    public decimal Productprice { get; set; }

    public string? Productimage { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
}
