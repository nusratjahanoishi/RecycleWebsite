using System.ComponentModel.DataAnnotations;

namespace NextUses.Models.Auth
{
    public class EmailModel
    {
        [Required(ErrorMessage = "Email Is Required:")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
