using Microsoft.AspNetCore.Mvc.Rendering;

namespace NextUses.Helper
{
    public class ConstantData
    {
        public const string Admin = "Admin";
        public const string Rider = "Rider";
        public const string User = "User";
        //

        public const string Active = "Active";
        public const string Deactive = "Deactive";
        //Product Status
        public const string Sell = "Sell";
        public const string Donate = "Donate";
        public const string Exchange = "Exchange"; 


        public static readonly List<string> RolesName = new()
        {
            Rider,
            User,
        }; 
        
        public static readonly List<string> Productstatus = new()
        {
            Sell,
            Donate,
            Exchange,
        };

        public static List<SelectListItem> GetStatusSelectList()
        {
            return RolesName.Select(status => new SelectListItem
            {
                Text = status,
                Value = status
            }).ToList();

        }

        public static List<SelectListItem> GetProductStatusSelectList()
        {
            return Productstatus.Select(status => new SelectListItem
            {
                Text = status,
                Value = status
            }).ToList();
        }
    }
}
