using Microsoft.AspNetCore.Mvc.Rendering;

namespace NextUses.Helper
{
    public class ConstantData
    {
        public const string Admin = "Admin";
        public const string Rider = "Rider";
        public const string User = "User";
        public static readonly List<string> RolesName = new()
        {
            Rider,
            User,
        };

        public static List<SelectListItem> GetStatusSelectList()
        {
            return RolesName.Select(status => new SelectListItem
            {
                Text = status,
                Value = status
            }).ToList();

        }
    }
}
