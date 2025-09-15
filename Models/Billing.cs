using System.ComponentModel.DataAnnotations;

namespace NextUses.Models
{
    public class Billing
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public string MobileNumber { get; set; }
        public string Country { get; set; }

        [Required]
        public string City { get; set; }
        public string AdditionalInfo { get; set; }
        public Users? Users { get; set; }
        // Relation to Orders
        public ICollection<Order>? Orders { get; set; }
    }
}
