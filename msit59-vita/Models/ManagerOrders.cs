namespace msit59_vita.Models
{
    public class ManagerOrders
    {
        public List<OrderViewModel> Orders { get; set; }

        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int CurrentPageIndex { get; set; }
        public int MaxRows { get; set; }

        public class OrderViewModel
        {
            public int OrderId { get; set; }
            public DateTime OrderTime { get; set; }
            public bool OrderPayment { get; set; }
            public bool OrderDeliveryVia { get; set; }
            public byte CustomerOrderStatus { get; set; }
            public DateTime? OrderFinishedTime { get; set; }
            public int CustomerId { get; set; }

            public int StoreId { get; set; }

        }
    }
}
