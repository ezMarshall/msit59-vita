namespace msit59_vita.Models
{
    public class ManagerProducts
    {
        public List<ProductViewModel> Products { get; set; }
        public List<ProductCategory> Categories { get; set; }
        public int PageCount { get; set; }
        public int CurrentPageIndex { get; set; }
        public int MaxRows { get; set; }

        public class ProductViewModel
        {
            public int ProductId { get; set; }
            public string ProductImage { get; set; }
            public required string ProductName { get; set; }
            public required decimal ProductUnitPrice { get; set; }
            public required short ProductUnitsInStock { get; set; }
            public bool ProductOnSell { get; set; }
            public required int CategoryId { get; set; }
            public required string CategoryName { get; set; }
        }

    }
}
