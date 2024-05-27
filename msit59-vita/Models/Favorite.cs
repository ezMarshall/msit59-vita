using System;
using System.Collections.Generic;

namespace msit59_vita.Models;

public partial class Favorite
{
    public int FavoriteId { get; set; }

    public int CustomerId { get; set; }

    public int StoreId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
