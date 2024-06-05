using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace msit59_vita.Models;

public partial class OrderMessage
{
    public int MessageId { get; set; }

    public int OrderId { get; set; }

    public DateTime MessageInformedTime { get; set; }

    public byte MessageContent { get; set; }

    public bool MessageStatus { get; set; }

    public virtual Order Order { get; set; } = null!;
}

