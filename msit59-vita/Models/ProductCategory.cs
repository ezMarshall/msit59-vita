using System;
using System.Collections.Generic;

namespace msit59_vita.Models;

public partial class ProductCategory
{
    public int CategoryId { get; set; }

    public int StoreId { get; set; }

    public string CategoryName { get; set; } = null!;

    public bool CategoryOnDelete { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual Store Store { get; set; } = null!;
}
