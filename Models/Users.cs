using Microsoft.AspNetCore.Identity;
using NextUses.Helper;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextUses.Models
{
    public class Users : IdentityUser
    {
      
        public string Name { get; set; }
        public string Role { get; set; } = ConstantData.User;
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public byte[]? Image { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
