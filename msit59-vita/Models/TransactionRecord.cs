using System;
using System.Collections.Generic;

namespace msit59_vita.Models;

public partial class TransactionRecord
{
    public int TransactionRecordsId { get; set; }

    public int OrderId { get; set; }

    public string TransactionId { get; set; } = null!;

    public DateTime TransactionTime { get; set; }

    public string TransactionTimestamp { get; set; } = null!;

    public byte TransactionType { get; set; }

    public bool TransactionResult { get; set; }

    public virtual Order Order { get; set; } = null!;
}
