using System;
using System.Collections.Generic;

namespace msit59_vita.Models;

public partial class TransactionRecord
{
    public int TransactionRecordId { get; set; }

    public int OrderID { get; set; }

    public string TransactionId { get; set; }

    public DateTime TransactionTime { get; set; }

    public string TransactionTimestamp { get; set; }

    public byte TransactionType { get; set; }

    public bool TransactionResult { get; set; }

    public virtual Order Order{ get; set; } = null!;

}
