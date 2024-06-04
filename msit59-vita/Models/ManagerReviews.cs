namespace msit59_vita.Models
{
    public class ManagerReviews
    {
   
        public List<ReviewViewModel> Reviews { get; set; }

         public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int CurrentPageIndex { get; set; }
        public int MaxRows { get; set; }

        public class ReviewViewModel
        {
            public int ReviewId { get; set; }
            public int OrderId { get; set; }
            public int CustomerId { get; set; }
            public string CustomerName { get; set; } = null!;
            public DateTime ReviewTime { get; set; }
            public string? ReviewContent { get; set; }
            public byte ReviewRating { get; set; }
            public string? StoreReplyContent { get; set; }
            public string ProductName { get; set; } = null!;
            public int StoreId { get; set; }

        }



    }
}
