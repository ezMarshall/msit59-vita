using System;
using System.Collections.Generic;

namespace msit59_vita.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string StoreName { get; set; } = null!;

    public string StoreAddressCity { get; set; } = null!;

    public string StoreAddressDistrict { get; set; } = null!;

    public string StoreAddressDetails { get; set; } = null!;

    public string? StoreAddressMemo { get; set; }

    public string StorePhoneNumber { get; set; } = null!;

    public string StoreImage { get; set; } = null!;

    public bool StoreUniformInvoiceVia { get; set; }

    public string StoreAccountNumber { get; set; } = null!;

    public string? StorePassword { get; set; }

    public DateTime StoreSetTime { get; set; }

    public byte StoreOrderStatus { get; set; }

    public byte StoreOnService { get; set; }

    public DateTime? StoreEndServiceTime { get; set; }

    public bool StoreLinePay { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<StoreOpeningHour> StoreOpeningHours { get; set; } = new List<StoreOpeningHour>();
}
