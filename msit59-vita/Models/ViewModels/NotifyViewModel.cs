namespace msit59_vita.Models.ViewModels
{
    public class NotifyViewModel
    {
        public int OrderId { get; set; }

        public DateTime MessageInformedTime { get; set; }

        public byte MessageContent { get; set; }

        public bool MessageStatus { get; set; }

        public bool DeliveryVia { get; set; }
    }
}
