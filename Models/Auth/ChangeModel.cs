using System.ComponentModel.DataAnnotations;

namespace NextUses.Models.Auth
{
    public class ChangeModel
    {
            [Required(ErrorMessage = "Email Is Required:")]
            [EmailAddress]
            public string Email { get; set; }
            public string Password { get; set; }
            [Required(ErrorMessage = "Password Is Required:")]
            [StringLength(40, MinimumLength = 8, ErrorMessage = "Password Must be 8 Digit.")]
            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            [Compare("ConfirmNewPassword", ErrorMessage = "Password Does Not Match.")]
            public string NewPassword { get; set; }
            [Required(ErrorMessage = "Confirm Password Is Required:")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm New Password")]
            public string ConfirmNewPassword { get; set; }
        }
    }
