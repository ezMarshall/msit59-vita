using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace msit59_vita.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "請輸入姓名")]
    public string CustomerName { get; set; } = null!;

    public string? CustomerNickName { get; set; }

    [Required(ErrorMessage = "請輸入電子信箱")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "請輸入有效的電子信箱")]
    public string CustomerEmail { get; set; } = null!;

    [RegularExpression(@"^09\d{8}$", ErrorMessage = "請輸入有效的手機號碼")]
    public string? CustomerCellPhone { get; set; }
  
    public string? CustomerLocalPhone { get; set; }

    public string? CustomerAddressCity { get; set; }

    public string? CustomerAddressDistrict { get; set; }

    public string? CustomerAddressDetails { get; set; }

    public string? CustomerAddressMemo { get; set; }

    public string? CustomerEinvoiceNumber { get; set; }

    [Required(ErrorMessage = "請輸入密碼")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,20}$", ErrorMessage = "請輸入有效的電子信箱")]
    public string CustomerPassword { get; set; } = null!;

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
}
