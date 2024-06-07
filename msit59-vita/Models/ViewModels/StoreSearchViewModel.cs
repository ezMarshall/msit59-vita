using System.Diagnostics;

namespace msit59_vita.Models.ViewModels
{
    public class StoreSearchViewModel
    {
        public string StoreSearch { get; set; } = null!;

        public int StoreId {  get; set; }
        public string StoreName { get; set; } = null!;

        public string StoreAddress { get; set; } = null!;

        public string StoreImage { get; set; } = null!;

        public byte StoreOrderStatus { get; set; }

        public string? StoreOpeningTime { get; set; }

        public bool StoreOpening { get; set; }

        public decimal averageRating {  get; set; }

        public int totalReviews { get; set; }

        public bool isFavorite {  get; set; }

        public double Dis {  get; set; }

        public List<string> ProductImageList { get; set; } = null!;
}
