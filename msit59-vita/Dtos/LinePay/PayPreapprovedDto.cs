namespace msit59_vita.Dtos
{
    public class PayPreapprovedDto
    {
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string OrderId { get; set; }
        public bool? Capture { get; set; }
    }
}

