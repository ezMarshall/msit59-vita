using System;
using System.Collections.Generic;

namespace msit59_vita.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int OrderId { get; set; }

    public DateTime ReviewTime { get; set; }

    public byte ReviewRating { get; set; }

    public string? ReviewContent { get; set; }

    public DateTime? StoreReplyTime { get; set; }

    public string? StoreReplyContent { get; set; }

    public virtual Order Order { get; set; } = null!;
}
