using System.ComponentModel.DataAnnotations;

namespace SRIJANWEBUI.Models
{
    public class AdminLogin
    {
        [Required(ErrorMessage = "Please Enter your Employee ID")]
        public string EmpId { get; set; }

        [Required(ErrorMessage = "Please Enter your password")]
        public string Password { get; set; }
        
        //public int? CompanyCode { get; set; } = 0;

        //public string CaptchaKey { get; set; } = string.Empty;

        //public bool IsRememberMe { get; set; }
    }
}
