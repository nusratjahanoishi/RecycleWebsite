using Microsoft.AspNetCore.Mvc.Rendering;

namespace NextUses.Helper
{
    public class Checkoutinfo
    {

        public const string Bkash = "Bkash";
        public const string Nagad = "Nagad";
        public const string COD = "Cash On Delivery";
        public const string Rocket = "Rocket";


        //Product Status
        public const string Pending = "Pending";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
        public const string AssignRider = "Assign Rider";
        public const string RejectedRider = "Rejected Rider";
   
        public static readonly List<string> PaymentType= new()
        {
            COD,
            Bkash,
            Nagad,
            Rocket,
        };

        public static readonly List<string> Paymentstatus = new()
        {
            Pending,
            Completed,
            AssignRider,
            RejectedRider,
            Cancelled,
        };

        public static List<SelectListItem> GetPaymentstatusSelectList()
        {
            return Paymentstatus.Select(status => new SelectListItem
            {
                Text = status,
                Value = status
            }).ToList();

        }

        public static List<SelectListItem> GetPaymentTypeSelectList()
        {
            return PaymentType.Select(status => new SelectListItem
            {
                Text = status,
                Value = status
            }).ToList();
        }
    }
}
