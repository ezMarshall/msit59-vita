using System;
using System.Collections.Generic;

namespace msit59_vita.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public int StoreId { get; set; }

    public DateTime OrderTime { get; set; }

    public DateTime PredictedArrivalTime { get; set; }

    public bool OrderDeliveryVia { get; set; }

    public string OrderPhoneNumber { get; set; } = null!;

    public string? OrderStoreMemo { get; set; }

    public bool OrderPayment { get; set; }

    public byte? OrderUniformInvoiceVia { get; set; }

    public string? OrderEinvoiceNumber { get; set; }

    public string OrderAddressCity { get; set; } = null!;

    public string OrderAddressDistrict { get; set; } = null!;

    public string OrderAddressDetails { get; set; } = null!;

    public string? OrderAddressMemo { get; set; }

    public byte CustomerOrderStatus { get; set; }

    public DateTime? OrderFinishedTime { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<OrderMessage> OrderMessages { get; set; } = new List<OrderMessage>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Store Store { get; set; } = null!;
}
