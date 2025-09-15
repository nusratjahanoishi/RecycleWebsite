using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextUses.Data;
using NextUses.Helper;
using NextUses.Models;
using NextUses.Models.ViewModel;

namespace NextUses.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly UserManager<Users> userManager;
        private readonly NextUsesDB context;

        public ProductController(NextUsesDB context, UserManager<Users> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var vm = new ProductViewModel
            {
                Product = new Product(),
                Categories = context.Categories.ToList()
            };

            return View(vm);
        }

            // POST: Create Product
            [HttpPost]
            [ValidateAntiForgeryToken]
        public IActionResult Create(ProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = context.Categories.ToList();
                TempData["SweetAlertMessage"] = "Product information Invalid!";
                TempData["SweetAlertIcon"] = "error";
                return View(vm);
            }

            // Main image
            if (vm.Product.ImageFile != null)
            {
                using var ms = new MemoryStream();
                vm.Product.ImageFile.CopyTo(ms);
                vm.Product.Image = ms.ToArray();
            }

            // Gallery images
            if (vm.Product.GalImageFiles != null && vm.Product.GalImageFiles.Length > 0)
            {
                vm.Product.GalImages = new List<ProductGallery>();
                foreach (var file in vm.Product.GalImageFiles)
                {
                    using var ms = new MemoryStream();
                    file.CopyTo(ms);
                    vm.Product.GalImages.Add(new ProductGallery { Image = ms.ToArray() });
                }
            }

            // Debugging check
            // Console.WriteLine("Saving Product => " + vm.Product.Name);
            vm.Product.StatusType = ConstantData.Active;
            context.Products.Add(vm.Product);
            context.SaveChanges();

            TempData["SweetAlertMessage"] = "Product added successfully!";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("Create");
        }

        [HttpGet]
        public IActionResult List()
        {
            var products = context.Products
                          .Include(p => p.Users)  
                          .Include(p => p.Category) 
                          .ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var product = context.Products
                     .Include(p => p.Category)
                     .Include(p => p.GalImages) // ✅ Gallery images Include
                     .FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();

            var vm = new ProductViewModel
            {
                Product = product,
                Categories = context.Categories.ToList()
            };

            return View(vm);

        }

        // POST: Edit Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = context.Categories.ToList();
                TempData["SweetAlertMessage"] = "Product update failed!";
                TempData["SweetAlertIcon"] = "error";
                return View(vm);
            }

            var product = context.Products
                                 .Include(p => p.GalImages)
                                 .FirstOrDefault(p => p.Id == vm.Product.Id);

            if (product == null) return NotFound();

            // Update fields
            product.Name = vm.Product.Name;
            product.Description = vm.Product.Description;
            product.Price = vm.Product.Price;
            product.ProductStatus = vm.Product.ProductStatus;
            product.CategoryId = vm.Product.CategoryId;

            // Update main image
            if (vm.Product.ImageFile != null)
            {
                using var ms = new MemoryStream();
                vm.Product.ImageFile.CopyTo(ms);
                product.Image = ms.ToArray();
            }

            if (vm.Product.GalImageFiles != null && vm.Product.GalImageFiles.Length > 0)
            {
                // Remove old gallery images from DB
                if (product.GalImages != null && product.GalImages.Any())
                {
                    context.ProductGalleries.RemoveRange(product.GalImages);
                }

                // Add new gallery images
                foreach (var file in vm.Product.GalImageFiles)
                {
                    using var ms = new MemoryStream();
                    file.CopyTo(ms);
                    var gallery = new ProductGallery
                    {
                        ProductId = product.Id, // Important: assign ProductId
                        Image = ms.ToArray()
                    };
                    context.ProductGalleries.Add(gallery);
                }
            }

            // Save all changes at once
            context.SaveChanges();

           

            TempData["SweetAlertMessage"] = "Product updated successfully!";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("List");
        }


        [HttpGet]
        public IActionResult Edituserproduct(Guid id)
        {
            var product = context.Products
                     .Include(p => p.Category)
                     .Include(p => p.GalImages) // ✅ Gallery images Include
                     .FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();

            var vm = new ProductViewModel
            {
                Product = product,
                Categories = context.Categories.ToList()
            };

            return View(vm);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edituserproduct(ProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = context.Categories.ToList();
                TempData["SweetAlertMessage"] = "Product update failed!";
                TempData["SweetAlertIcon"] = "error";
                return View(vm);
            }

            var product = context.Products
                                 .Include(p => p.GalImages)
                                 .FirstOrDefault(p => p.Id == vm.Product.Id);

            if (product == null) return NotFound();

            // Update fields
            product.Name = vm.Product.Name;
            product.Description = vm.Product.Description;
            product.Price = vm.Product.Price;
            product.ProductStatus = vm.Product.ProductStatus;
            product.CategoryId = vm.Product.CategoryId;

            // Update main image
            if (vm.Product.ImageFile != null)
            {
                using var ms = new MemoryStream();
                vm.Product.ImageFile.CopyTo(ms);
                product.Image = ms.ToArray();
            }

            if (vm.Product.GalImageFiles != null && vm.Product.GalImageFiles.Length > 0)
            {
                // Remove old gallery images from DB
                if (product.GalImages != null && product.GalImages.Any())
                {
                    context.ProductGalleries.RemoveRange(product.GalImages);
                }

                // Add new gallery images
                foreach (var file in vm.Product.GalImageFiles)
                {
                    using var ms = new MemoryStream();
                    file.CopyTo(ms);
                    var gallery = new ProductGallery
                    {
                        ProductId = product.Id, // Important: assign ProductId
                        Image = ms.ToArray()
                    };
                    context.ProductGalleries.Add(gallery);
                }
            }

            // Save all changes at once
            context.SaveChanges();

           

            TempData["SweetAlertMessage"] = "Product updated successfully!";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("Usersellproduct", "Product", new { id = vm.Product.UserId });

        }


        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var product = context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            context.Products.Remove(product);
            context.SaveChanges();

            TempData["SweetAlertMessage"] = "Product deleted successfully!";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Deleteuserproduct(Guid id)
        {
            var product = context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            context.Products.Remove(product);
            context.SaveChanges();

            TempData["SweetAlertMessage"] = "Product deleted successfully!";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("Usersellproduct");
        }

        [HttpGet]
        public IActionResult Usersellproduct(string id)
        {
            
            if (string.IsNullOrEmpty(id))
            {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            
            var products = context.Products
                                   .Include(p => p.Category)
                                    .Include(p => p.Users)
                                   .Where(p => p.UserId == id)
                                   .ToList();

            return View(products); 
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Statuschange(Guid ProductId, string StatusType)
        {
            if (string.IsNullOrEmpty(StatusType))
            {
                TempData["SweetAlertMessage"] = "Please select a valid status!";
                TempData["SweetAlertIcon"] = "error";
                return RedirectToAction("List"); // Redirect back to order list
            }

            var order = context.Products.FirstOrDefault(o => o.Id == ProductId);
            if (order == null)
            {
                TempData["SweetAlertMessage"] = "Product not found!";
                TempData["SweetAlertIcon"] = "error";
                return RedirectToAction("List");
            }

            order.StatusType = StatusType;
            context.SaveChanges();

            TempData["SweetAlertMessage"] = $"Product status updated to {StatusType} successfully!";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("List");
        }
    }

}
