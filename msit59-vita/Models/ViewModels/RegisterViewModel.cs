using System.ComponentModel.DataAnnotations;

namespace msit59_vita.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "請輸入姓名")]
        public string CustomerName { get; set; } = null!;

        public string? CustomerNickName { get; set; }

        [Required(ErrorMessage = "請輸入電子信箱")]
        [EmailAddress(ErrorMessage = "請輸入有效的電子信箱")]
        public string CustomerEmail { get; set; } = null!;

        [Phone(ErrorMessage = "請輸入有效的手機號碼")]
        public string? CustomerCellPhone { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "請輸入密碼")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,20}$", ErrorMessage = "請輸入有效的電子信箱")]
        public string CustomerPassword { get; set; } = null!;

        [Required(ErrorMessage = "請輸入確認密碼")]
        [DataType(DataType.Password)]
        [Compare("CustomerPassword", ErrorMessage = "確認密碼與密碼不一致")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
