using System.ComponentModel.DataAnnotations;
using NextUses.Helper;

namespace NextUses.Models.ViewModel
{
    public class PlaceOrderViewModel
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, StringLength(20)]
        public string MobileNumber { get; set; }

        public string? TaxId { get; set; }

        [Required]
        public string City { get; set; }

        public string Country { get; set; }

        public string AdditionalInfo { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = Checkoutinfo.COD;

        public string Status { get; set; } = Checkoutinfo.Pending;
    }
}
