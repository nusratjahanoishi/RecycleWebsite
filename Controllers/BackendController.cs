using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextUses.Data;
using NextUses.Helper;
using NextUses.Models;
using NextUses.Models.ViewModel;

namespace NextUses.Controllers
{
    public class BackendController : Controller
    {

        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;
        private readonly NextUsesDB context;

        private string? username;

        public BackendController(
            SignInManager<Users> signInManager,
            UserManager<Users> userManager,
            NextUsesDB context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
        
            var user = userManager.GetUserAsync(User).Result;   // Logged in user
            var roles = userManager.GetRolesAsync(user).Result;

            DashboardViewModel model = new DashboardViewModel();

            if (roles.Contains(ConstantData.Admin))
            {
                // ---------------------------
                // Admin Dashboard
                // ---------------------------
                model.TotalUsers = context.Users.Count();
                model.TotalRiders = context.Users.Count(u => u.Role == "Rider");

                model.TotalCategories = context.Categories.Count();

                model.ActiveProducts = context.Products.Count(p => p.StatusType == ConstantData.Active);
                model.DeactiveProducts = context.Products.Count(p => p.StatusType == ConstantData.Deactive);

                model.PendingOrders = context.Orders.Count(o => o.Status == Checkoutinfo.Pending);
                model.CompleteOrders = context.Orders.Count(o => o.Status == Checkoutinfo.Completed);
                model.RiderAssignedOrders = context.Orders.Count(o => o.Status == Checkoutinfo.AssignRider);
                model.RiderRejectedOrders = context.Orders.Count(o => o.Status == Checkoutinfo.RejectedRider);
                model.CancelledOrders = context.Orders.Count(o => o.Status == Checkoutinfo.Cancelled);

                model.PendingRiderApply = context.RiderApplications.Count(r => r.Status == "Pending");
                model.AcceptedRiderApply = context.RiderApplications.Count(r => r.Status == "Approved");
                model.RejectedRiderApply = context.RiderApplications.Count(r => r.Status == "Rejected");
            }
            else if (roles.Contains(ConstantData.Rider))
            {
                // ---------------------------
                // Rider Dashboard
                // ---------------------------
                var riderId = user.Id; // Current Rider Id

                // Orders
                model.PendingOrders = context.Orders.Count(o => o.Status == Checkoutinfo.Pending);
                model.CompleteOrders = context.Orders.Count(o => o.Status == Checkoutinfo.Completed);
                // Rider Applications
                model.PendingRiderApply = context.RiderApplications
                    .Count(r => r.Status == "Pending" && r.RiderId == riderId);

                model.AcceptedRiderApply = context.RiderApplications
                    .Count(r => r.Status == "Approved" && r.RiderId == riderId);

                model.RejectedRiderApply = context.RiderApplications
                    .Count(r => r.Status == "Rejected" && r.RiderId == riderId);
            }


            return View(model);

           

        }
    }
}
