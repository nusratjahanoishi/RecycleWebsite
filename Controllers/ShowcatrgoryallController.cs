using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NextUses.Data;
using NextUses.Helper;
using NextUses.Models;
using Microsoft.EntityFrameworkCore;
using NextUses.Models.ViewModel;

namespace NextUses.Controllers
{
    public class ShowcatrgoryallController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;
        private readonly NextUsesDB context;
        private string? username;

        public ShowcatrgoryallController(NextUsesDB context,
            SignInManager<Users> signInManager,
            UserManager<Users> userManager)

        {
            this.context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;

        }
        public IActionResult Showcategory(Guid id)
        {
            var vm = new FrontendModel
            {
                Products = context.Products
                  .Include(p => p.Category)
                  .Where(p => p.StatusType == ConstantData.Active && p.CategoryId == id)
                  .ToList()
            };

            return View(vm);
        }
    }
}
