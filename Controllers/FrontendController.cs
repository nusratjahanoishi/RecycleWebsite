using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NextUses.Models;

namespace NextUses.Controllers
{
    public class FrontendController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;

        private string? username;

        public FrontendController(
            SignInManager<Users> signInManager,
            UserManager<Users> userManager)

        {
            this.signInManager = signInManager;
            this.userManager = userManager;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
