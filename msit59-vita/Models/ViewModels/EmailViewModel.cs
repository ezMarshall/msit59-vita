using System.ComponentModel.DataAnnotations;

namespace msit59_vita.Models.ViewModels
{
    public class EmailViewModel
    {
        [Required(ErrorMessage = "請輸入電子信箱")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子信箱")]
        public string CustomerEmail { get; set; } = null!;

        public bool? Success { get; set; }

        public string? Message { get; set; }
        public string? FromEmail { get; set; } = null!;
        public string? Subject { get; set; } = null!;
        public string? EmailBody { get; set; } = null!;
    }
}
