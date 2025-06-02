using System.ComponentModel.DataAnnotations;

namespace SRIJANWEBUI.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Please Enter Your Customer Id")]
        public string CustomerId { get; set; }

        [Required(ErrorMessage ="Please Enter Your Password")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Company Code is required")]
        public int CompanyCode { get; set; }

        //public string CaptchaKey { get; set; } = string.Empty;

        //public bool IsRememberMe { get; set; }
    }
}
