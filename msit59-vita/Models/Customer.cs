using System;
using System.Collections.Generic;

namespace msit59_vita.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string? CustomerNickName { get; set; }

    public string CustomerEmail { get; set; } = null!;

    public string? CustomerCellPhone { get; set; }

    public string? CustomerLocalPhone { get; set; }

    public string CustomerAddressCity { get; set; } = null!;

    public string CustomerAddressDistrict { get; set; } = null!;

    public string CustomerAddressDetails { get; set; } = null!;

    public string? CustomerAddressMemo { get; set; }

    public string? CustomerEinvoiceNumber { get; set; }

    public string CustomerPassword { get; set; } = null!;

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}
