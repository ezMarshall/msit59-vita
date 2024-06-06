using System.ComponentModel.DataAnnotations;

namespace msit59_vita.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "請輸入電子信箱")]
        [EmailAddress]
        public string CustomerEmail { get; set; } = null!;

        [Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        public string CustomerPassword { get; set; } = null!;
    }
}
