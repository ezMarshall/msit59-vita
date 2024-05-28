using System;
using System.Collections.Generic;

namespace msit59_vita.Models;

public partial class StoreOpeningHour
{
    public int StoreId { get; set; }

    public string MyWeekDay { get; set; } = null!;

    public bool? StoreOpenOrNot { get; set; }

    public TimeOnly? StoreOpeningTime { get; set; }

    public TimeOnly? StoreClosingTime { get; set; }

    public virtual Store Store { get; set; } = null!;
}
