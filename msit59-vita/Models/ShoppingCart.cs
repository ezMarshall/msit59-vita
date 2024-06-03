using System;
using System.Collections.Generic;

namespace msit59_vita.Models;

public partial class ShoppingCart
{
    public int ShoppingCartId { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public short ShoppingCartQuantity { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
