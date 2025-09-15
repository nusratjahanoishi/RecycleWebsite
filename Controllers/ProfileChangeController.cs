using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NextUses.Data;
using NextUses.Models;

namespace NextUses.Controllers
{
    [Authorize]
    public class ProfileChangeController : Controller
    {

        private readonly UserManager<Users> userManager;
        private readonly NextUsesDB context;

        public ProfileChangeController(NextUsesDB context, UserManager<Users> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Information(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // Profile update
        [HttpPost]
        public async Task<IActionResult> Information(string id, Users model)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // Update fields
            user.Name = model.Name;
            user.Address = model.Address;
            user.Gender = model.Gender;
            user.DateOfBirth = model.DateOfBirth;
            user.Phone = model.Phone;

            // Image update
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await model.ImageFile.CopyToAsync(ms);
                    user.Image = ms.ToArray();
                }
            }

            await userManager.UpdateAsync(user);

            TempData["SweetAlertMessage"] = "Profile updated successfully!";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("Information", new { id = user.Id });
        }
    }
}
