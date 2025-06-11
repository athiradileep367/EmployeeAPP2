
using System.ComponentModel.DataAnnotations;

namespace EmployeeAPP2.Models
{
    public class LoginViewModel
    {
        
        
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[\W_]).{6,}$", ErrorMessage = "Password must include at least one special character")]
        
        public string Password { get; set; }
    }
}
