using System.ComponentModel.DataAnnotations;

namespace msit59_vita.Models.ViewModels
{
    public class ManagerLoginViewModel
    {
        [Required(ErrorMessage = "請輸入帳號")]
        public string StoreAccountNumber { get; set; } = null!;

        [Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        public string StorePassword { get; set; } = null!;
    }
}
