using System;
using System.Collections.Generic;

namespace _24dh111520_LTW.Models;

public partial class Customer
{
    public int Customerid { get; set; }

    public string Customername { get; set; } = null!;

    public string Customerphone { get; set; } = null!;

    public string? Customeremail { get; set; }

    public string? Customeraddress { get; set; }

    public string Username { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User UsernameNavigation { get; set; } = null!;
}
