using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NextUses.Data;
using NextUses.Models.ViewModel;
using NextUses.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using NextUses.Helper;

namespace NextUses.Controllers
{
    public class OrderController : Controller
    {
        private readonly UserManager<Users> userManager;
        private readonly NextUsesDB context;

        public OrderController(NextUsesDB context, UserManager<Users> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin,Rider")]
        public IActionResult OrderList()
        {
            var orders = context.Orders
                .Include(o => o.Product)
                .Include(o => o.Billing)
                    .ThenInclude(b => b.Users)
                .Include(o => o.RiderApplications)
                    .ThenInclude(ra => ra.Rider) // Rider এর নাম পেতে
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Rider")]
        public IActionResult OrderList(Guid OrderId, string newStatus)
        {
            if (string.IsNullOrEmpty(newStatus))
            {
                TempData["SweetAlertMessage"] = "Please select a valid status!";
                TempData["SweetAlertIcon"] = "error";
                return RedirectToAction("OrderList"); // Redirect back to order list
            }

            var order = context.Orders.FirstOrDefault(o => o.Id == OrderId);
            if (order == null)
            {
                TempData["SweetAlertMessage"] = "Order not found!";
                TempData["SweetAlertIcon"] = "error";
                return RedirectToAction("OrderList");
            }

            order.Status = newStatus;
            context.SaveChanges();

            TempData["SweetAlertMessage"] = $"Order status updated to {newStatus} successfully!";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("OrderList");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Rider")]

        public IActionResult ApplyForOrder(Guid orderId)
        {
            var order = context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) return NotFound();

            // Login করা Rider Id নেব
            var riderId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(riderId))
                return Unauthorized();

            // Product ও লোড করব
            var product = context.Products.FirstOrDefault(p => p.Id == order.ProductId);

            // Rider Application Save
            var application = new RiderApplication
            {
                OrderId = orderId,
                RiderId = riderId,
                Status = "Pending"
            };

            context.RiderApplications.Add(application);

            // Order Status change
            order.Status = Checkoutinfo.AssignRider;

            // Product Status change
            if (product != null)
            {
                product.StatusType = ConstantData.Deactive;
            }

            context.SaveChanges();

            TempData["SweetAlertMessage"] = "You have applied for this order!";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("OrderList");
        }


    }
}
