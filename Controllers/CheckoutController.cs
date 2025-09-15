using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NextUses.Data;
using NextUses.Models.ViewModel;
using NextUses.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using NextUses.Helper;
using Microsoft.AspNetCore.Authorization;

namespace NextUses.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly UserManager<Users> userManager;
        private readonly NextUsesDB context;

        public CheckoutController(NextUsesDB context, UserManager<Users> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(Guid id)
        {
            // Find the product by ID
            var product = context.Products.FirstOrDefault(p => p.Id == id);

            // If product not found, return 404
            if (product == null)
                return NotFound();

            // Prepare the ViewModel
            var viewModel = new CheckoutPageViewModel
            {
                Product = product,
                Order = new PlaceOrderViewModel
                {
                    ProductId = product.Id,
                }
            };

            // Return ViewModel to the view
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder(CheckoutPageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SweetAlertMessage"] = "Invalid input data. Please try again.";
                TempData["SweetAlertIcon"] = "error";
                return View("Index", model);
            }

            var product = context.Products.FirstOrDefault(p => p.Id == model.Order.ProductId);
            if (product == null) return NotFound();

            var billing = new Billing
            {
                Id = Guid.NewGuid(),
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                MobileNumber = model.Order.MobileNumber,
                Country = model.Order.Country,
                City = model.Order.City,
                AdditionalInfo = model.Order.AdditionalInfo
            };

            context.Billings.Add(billing);
           

            var order = new Order
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                BillingId = billing.Id,
                PaymentMethod = model.Order.PaymentMethod,
                TaxId = model.Order.TaxId,
                Status = model.Order.Status,
                OrderDate = DateTime.Now
            };

            context.Orders.Add(order);
            context.SaveChanges(); // save order

            TempData["SweetAlertMessage"] = $"🎉 Your order has been placed successfully! Order ID: {order.Id}";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("Index", "Frontend");
        }

        [Authorize]
        public IActionResult OrderList()
        {
            // Admin hole shob order, user hole tar nijer order
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = context.Orders
          .Include(o => o.Product)
          .Include(o => o.Billing)
          .Where(o => o.Billing.UserId == userId)   // ✅ Only this user’s orders
          .OrderByDescending(o => o.OrderDate)
          .ToList();

            return View(orders);
        }


    }
}
