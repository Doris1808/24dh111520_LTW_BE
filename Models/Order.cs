using System;
using System.Collections.Generic;

namespace _24dh111520_LTW.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public int Customerid { get; set; }

    public DateOnly Orderdate { get; set; }

    public decimal Totalamount { get; set; }

    public string? Paymentstatus { get; set; }

    public string Addressdelivery { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
}
