using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NextUses.Models.Auth;
using NextUses.Models;
using NextUses.Helper;


namespace Bachelor_management_software.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;
        private string? username;

        public AccountController(
            SignInManager<Users> signInManager,
            UserManager<Users> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpGet]
        // GET: Login
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                
                var user = userManager.GetUserAsync(User).Result;
                var roles = userManager.GetRolesAsync(user).Result;

                if (roles.Contains("Admin") || roles.Contains("Rider"))
                    return RedirectToAction("Index", "Backend");
                else
                    return RedirectToAction("Index", "Frontend");
            }

            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SweetAlertMessage"] = "User not found.";
                TempData["SweetAlertIcon"] = "error";
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                var roles = await userManager.GetRolesAsync(user);

                if (roles.Contains("Admin") || roles.Contains("Rider"))
                {
                    if (roles.Contains("Admin"))
                    {
                        TempData["SweetAlertMessage"] = "Admin Page Login Successful!";
                        TempData["SweetAlertIcon"] = "success";
                    }
                    else
                    {
                        TempData["SweetAlertMessage"] = "Rider Page Login Successful!";
                        TempData["SweetAlertIcon"] = "success";
                    }

                    return RedirectToAction("Index", "Backend");
                }
                else
                {
                    TempData["SweetAlertMessage"] = "User Login Successful!";
                    TempData["SweetAlertIcon"] = "success";
                    return RedirectToAction("Index", "Frontend");
                }
            }
            else
            {
                TempData["SweetAlertMessage"] = "Email or Password is incorrect.";
                TempData["SweetAlertIcon"] = "error";
                return View(model);
            }
        }



        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    Role = ConstantData.User.ToString()
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    await userManager.AddToRoleAsync(user, ConstantData.User);
                    TempData["SweetAlertMessage"] = "Registration Successful!";
                    TempData["SweetAlertIcon"] = "success";
                    return RedirectToAction("Login", "Account");
                }
            }

            return View(model);
        }

      
        public IActionResult VerifyEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    TempData["SweetAlertMessage"] = "Very Sad!! Email not found.";
                    TempData["SweetAlertIcon"] = "error";
                    return View(model);
                }
                else
                {

                    TempData["SweetAlertMessage"] = "Email Found Successful Done.";
                    TempData["SweetAlertIcon"] = "success";
                    return RedirectToAction("Changepass", "Account", new { username = user.UserName });
                }
            }
            return View(model);
        }
        [HttpGet]

        public IActionResult Changepass(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                TempData["SweetAlertMessage"] = "Verify Email Successful Done!";
                TempData["SweetAlertIcon"] = "success";
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangeModel { Email = username });
        }
        [HttpPost]
        public async Task<IActionResult> Changepass(ChangeModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddPasswordAsync(user, model.NewPassword);
                        TempData["SweetAlertMessage"] = "Password changed successfully!";
                        TempData["SweetAlertIcon"] = "success";
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        TempData["SweetAlertMessage"] = "Email Not Found";
                        TempData["SweetAlertIcon"] = "error";
                        return View(model);
                    }
                }
                else
                {
                    TempData["SweetAlertMessage"] = "Something Sent Wrong. Try Again";
                    TempData["SweetAlertIcon"] = "error";
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            HttpContext.SignOutAsync();
            TempData["SweetAlertMessage"] = "Account Logout Successfully";
            TempData["SweetAlertIcon"] = "success";
            return RedirectToAction("Login", "Account");
        }

      

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
