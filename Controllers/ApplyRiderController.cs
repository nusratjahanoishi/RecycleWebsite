using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextUses.Data;
using NextUses.Helper;
using NextUses.Models;

namespace NextUses.Controllers
{
    public class ApplyRiderController : Controller
    {
        private readonly UserManager<Users> userManager;
        private readonly NextUsesDB context;

        public ApplyRiderController(NextUsesDB context, UserManager<Users> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin,Rider")]
        public IActionResult RiderApplyList()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var roles = User.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value)
                            .ToList();

            // Base query: RiderApplications include related Order, Product, and Rider(User)
            IQueryable<RiderApplication> query = context.RiderApplications
                .Include(ra => ra.Order)
                    .ThenInclude(o => o.Product) // Load Product info
                .Include(ra => ra.Rider) // Load Rider info
                .Include(ra => ra.Order)
                    .ThenInclude(o => o.Billing)
                        .ThenInclude(b => b.Users); // Load Billing User info

            if (roles.Contains("Rider"))
            {
                // Rider sees only his own applications
                query = query.Where(ra => ra.RiderId == userId);
            }

            var applications = query
                .OrderByDescending(ra => ra.AppliedAt)
                .ToList();

            return View(applications); // Pass RiderApplication list to View
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateRiderStatus(Guid applicationId, string newStatus)
        {
            var application = context.RiderApplications
                .Include(ra => ra.Order)
                .ThenInclude(o => o.Product)
                .FirstOrDefault(ra => ra.Id == applicationId);

            if (application == null)
            {
                TempData["SweetAlertMessage"] = "Application not found!";
                TempData["SweetAlertIcon"] = "error";
                return RedirectToAction("RiderApplyList");
            }

            // RiderApplication status update
            application.Status = newStatus;

           
            if (newStatus == "Rejected")
            {
                if (application.Order != null)
                {
                   
                    application.Order.Status = Checkoutinfo.RejectedRider;

                  
                    if (application.Order.Product != null)
                    {
                        application.Order.Product.StatusType = ConstantData.Active;
                    }
                }
            }

            context.SaveChanges();

            TempData["SweetAlertMessage"] = "Rider status updated successfully!";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("RiderApplyList");
        }


    }
}
