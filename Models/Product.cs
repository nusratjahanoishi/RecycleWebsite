using NextUses.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NextUses.Helper;
namespace NextUses.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required]
        public Guid CategoryId { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? Description { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }
        public string ProductStatus { get; set; }
        public string StatusType { get; set; } = ConstantData.Active;

        public byte[]? Image { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [NotMapped]
        public IFormFile[]? GalImageFiles { get; set; }

        public List<ProductGallery>? GalImages { get; set; } = new List<ProductGallery>();

        public Category? Category { get; set; }
        public Users? Users { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }

}