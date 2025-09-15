using NextUses.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace NextUses.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public byte[]? Image { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        // Relation
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}