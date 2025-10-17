using System;
using System.Collections.Generic;

namespace _24dh111520_LTW.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string CustomerPhone { get; set; } = null!;

    public string? CustomerEmail { get; set; }

    public string? CustomerAddress { get; set; }

    public string Username { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User UsernameNavigation { get; set; } = null!;
}
