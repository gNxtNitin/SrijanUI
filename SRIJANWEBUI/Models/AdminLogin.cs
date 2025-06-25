using System.ComponentModel.DataAnnotations;

namespace SRIJANWEBUI.Models
{
    public class AdminLogin
    {
        [Required(ErrorMessage = "Employee ID required")]
        public string EmpId { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
        
        //public int? CompanyCode { get; set; } = 0;

        //public string CaptchaKey { get; set; } = string.Empty;

        //public bool IsRememberMe { get; set; }
    }
}
