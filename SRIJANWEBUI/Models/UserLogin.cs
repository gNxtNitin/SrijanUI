using System.ComponentModel.DataAnnotations;

namespace SRIJANWEBUI.Models
{
    public class UserLogin
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public int CompanyCode { get; set; }

        public string CaptchaKey { get; set; } = string.Empty;

        public bool IsRememberMe { get; set; }
    }
}
