using System;
using System.Collections.Generic;

namespace _24dh111520_LTW.Models;

public partial class Orderdetail
{
    public int Id { get; set; }

    public int Productid { get; set; }

    public int Orderid { get; set; }

    public int Quantity { get; set; }

    public decimal Unitprice { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
