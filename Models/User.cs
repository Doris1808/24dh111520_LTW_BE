using System;
using System.Collections.Generic;

namespace _24dh111520_LTW.Models;

public partial class User
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public char UserRole { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
