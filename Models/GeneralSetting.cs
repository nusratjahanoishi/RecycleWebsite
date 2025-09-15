using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextUses.Models
{
    public class GeneralSetting
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string SiteName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string FooterText { get; set; } = string.Empty;

        [MaxLength(500)]
        public string SiteDescription { get; set; } = string.Empty;

        [MaxLength(200)]
        public string ContactEmail { get; set; } = string.Empty;

        [MaxLength(20)]
        public string ContactPhone { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        public byte[]? Logo { get; set; }

        [NotMapped]
        public IFormFile? LogoFile { get; set; }
    }
}
