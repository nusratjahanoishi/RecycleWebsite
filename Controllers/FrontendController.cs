using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextUses.Data;
using NextUses.Helper;
using NextUses.Models;
using NextUses.Models.ViewModel;

namespace NextUses.Controllers
{
    public class FrontendController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;
        private readonly NextUsesDB context;
        private string? username;

        public FrontendController(NextUsesDB context,
            SignInManager<Users> signInManager,
            UserManager<Users> userManager)

        {
            this.context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;

        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = new FrontendModel
            {
                Categories = context.Categories.ToList(),
                Products = context.Products.Where(p => p.StatusType == ConstantData.Active).ToList(),

            };

            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> About()
        {
            return View();
        }        
        
        [HttpGet]
        public async Task<IActionResult> Contact()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ProductDetails(Guid id)
        {
            var product = context.Products
                                  .Include(p => p.Category)
                                  .Include(p => p.GalImages) // gallery images
                                  .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> AllProduct()
        {
            var vm = new FrontendModel
            {
                Categories = context.Categories.ToList(),
                Products = context.Products.Where(p => p.StatusType == ConstantData.Active).ToList(),

            };

            return View(vm);
        }
    }
}
