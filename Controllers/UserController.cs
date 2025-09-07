using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NextUses.Data;
using NextUses.Helper;
using NextUses.Models;

namespace NextUses.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<Users> userManager;
        private readonly NextUsesDB context;

        public UserController(NextUsesDB context, UserManager<Users> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult List()
        {
            var users = context.Users.ToList();
            return View(users);
        }   
        [HttpPost]
        public async Task<IActionResult> Changerole(string UserId, string newRole)
        {
            var user = await userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return RedirectToAction("List");
            }


            // Remove old roles
            var currentRoles = await userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                await userManager.RemoveFromRolesAsync(user, currentRoles);
            }

            // Add new role to Identity
            await userManager.AddToRoleAsync(user, newRole);
            user.Role = newRole;
            await userManager.UpdateAsync(user);
            TempData["SweetAlertMessage"] = "Role Change successfully!";
            TempData["SweetAlertIcon"] = "success";
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Deleteuser(string id)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return RedirectToAction("List");
            }
            context.users.Remove(user);
            context.SaveChanges();
            TempData["SweetAlertMessage"] = "User deleted successfully!";
            TempData["SweetAlertIcon"] = "success";
            return RedirectToAction("List");
        }
        public IActionResult Riderlist()
        {
            var users = context.Users.Where(u => u.Role == ConstantData.Rider).ToList();
            return View(users);
        }

    }
}
