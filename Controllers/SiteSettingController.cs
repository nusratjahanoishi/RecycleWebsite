using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextUses.Data;
using NextUses.Models;

namespace NextUses.Controllers
{
    [Authorize]
    public class SiteSettingController : Controller
    {
        private readonly UserManager<Users> userManager;
        private readonly NextUsesDB context;

        public SiteSettingController(NextUsesDB context, UserManager<Users> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        
        [HttpGet]
        public IActionResult Setting()
        {
            var setting = context.GeneralSettings.FirstOrDefault();
            if (setting == null)
            {
                setting = new GeneralSetting(); // New for Create
            }
            return View(setting);
        }

        [HttpPost]
        public IActionResult Setting(GeneralSetting model)
        {
            if (ModelState.IsValid)
            {
                var setting = context.GeneralSettings.FirstOrDefault();

                if (setting == null)
                {
                    // Create new
                    if (model.LogoFile != null)
                    {
                        using var ms = new MemoryStream();
                        model.LogoFile.CopyTo(ms);
                        model.Logo = ms.ToArray();
                    }
                    context.GeneralSettings.Add(model);
                }
                else
                {
                    // Update
                    setting.SiteName = model.SiteName;
                    setting.FooterText = model.FooterText;
                    setting.SiteDescription = model.SiteDescription;
                    setting.ContactEmail = model.ContactEmail;
                    setting.ContactPhone = model.ContactPhone;
                    setting.Address = model.Address;

                    if (model.LogoFile != null)
                    {
                        using var ms = new MemoryStream();
                        model.LogoFile.CopyTo(ms);
                        setting.Logo = ms.ToArray();
                    }

                    context.GeneralSettings.Update(setting);
                }
                context.SaveChanges();
                return RedirectToAction("Setting");
            }

            return View(model);
        }
    }
}
