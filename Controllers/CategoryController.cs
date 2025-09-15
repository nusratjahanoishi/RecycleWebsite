using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextUses.Data;
using NextUses.Models;
using NextUses.Models.ViewModel;

namespace NextUses.Controllers
{
    public class CategoryController : Controller
    {
        private readonly UserManager<Users> userManager;
        private readonly NextUsesDB context;

        public CategoryController(NextUsesDB context, UserManager<Users> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Create()
        {

            var category = new CategoryView
            {

                Categories = context.Categories.ToList()
            };

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryView model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = context.Categories.ToList();
                TempData["SweetAlertMessage"] = "Category information error!";
                TempData["SweetAlertIcon"] = "error";
                return RedirectToAction("Create");
            }

            // Guid generate করো যদি Empty থাকে
            if (model.Category.Id == Guid.Empty)
                model.Category.Id = Guid.NewGuid();

            // Image Save
            if (model.Category.ImageFile != null)
            {
                using var ms = new MemoryStream();
                model.Category.ImageFile.CopyTo(ms);
                model.Category.Image = ms.ToArray();
            }

            context.Categories.Add(model.Category);
            context.SaveChanges();

            TempData["SweetAlertMessage"] = "Category added successfully!";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("Create");
        }



        // ✅ Edit GET
        public IActionResult Edit(Guid id)
        {
            var category = context.Categories.Find(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // ✅ Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Category model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                // ডাটাবেস থেকে category fetch করা
                var category = context.Categories.Find(id);
                if (category == null)
                    return NotFound();

                // Field update
                category.Name = model.Name;
                category.Description = model.Description;

                // Image update (যদি নতুন file থাকে)
                if (model.ImageFile != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        model.ImageFile.CopyTo(ms);
                        category.Image = ms.ToArray();
                    }
                }

                context.Categories.Update(category);
                context.SaveChanges();

                TempData["SweetAlertMessage"] = "Category updated successfully!";
                TempData["SweetAlertIcon"] = "success";

                // Safe redirect
                return RedirectToAction("Create");
            }

            TempData["SweetAlertMessage"] = "Category update error!";
            TempData["SweetAlertIcon"] = "error";

            return View(model);
        }

        public IActionResult Delete(Guid id)
        {
            var category = context.Categories.Find(id);
            if (category == null) return NotFound();

            context.Categories.Remove(category);
            context.SaveChanges();

            TempData["SweetAlertMessage"] = "Category Delete successfully!";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("Create");
        }

    }
}
