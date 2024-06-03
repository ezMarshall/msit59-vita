using Microsoft.AspNetCore.Identity;

namespace msit59_vita.Models
{
    public class VitaUser : IdentityUser
    {
        public string? VitaUserName { get; set; }
    }
}
