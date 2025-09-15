using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NextUses.Helper;

namespace NextUses.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [Required]
        public Guid BillingId { get; set; }

        [ForeignKey("BillingId")]
        public Billing? Billing { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = Checkoutinfo.COD;// COD / SSL / Stripe
        public string? TaxId { get; set; } 

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; } = Checkoutinfo.Pending; // Pending, Completed, Cancelled
        public ICollection<RiderApplication>? RiderApplications { get; set; }
    }
}
