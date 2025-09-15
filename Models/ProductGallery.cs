using System.ComponentModel.DataAnnotations;

namespace NextUses.Models
{
    public class ProductGallery
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public byte[] Image { get; set; }

        [Required]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
    }

}
