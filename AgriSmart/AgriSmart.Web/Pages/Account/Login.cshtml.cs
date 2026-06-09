using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AgriSmart.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AgriSmart.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Intercept simulated social login requests
            if (Input.Username == "google_user" || Input.Username == "facebook_user")
            {
                var socialUser = await _userManager.FindByNameAsync(Input.Username);
                if (socialUser == null)
                {
                    socialUser = new ApplicationUser
                    {
                        UserName = Input.Username,
                        Email = Input.Username + "@agrismart.pk",
                        FullName = Input.Username == "google_user" ? "Google User" : "Facebook User",
                        Region = "Punjab",
                        EmailConfirmed = true,
                        IsActive = true
                    };
                    var createResult = await _userManager.CreateAsync(socialUser, "OAuth@1234");
                    if (createResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(socialUser, "Farmer");
                    }
                    else
                    {
                        ErrorMessage = "Failed to create simulated social user.";
                        return Page();
                    }
                }

                await _signInManager.SignInAsync(socialUser, isPersistent: false);
                return Redirect("/farmer/dashboard");
            }

            var user = await _userManager.FindByNameAsync(Input.Username)
                ?? await _userManager.FindByEmailAsync(Input.Username);

            if (user == null || !user.IsActive)
            {
                ErrorMessage = "Invalid login attempt.";
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                    return Redirect("/admin/panel");
                return Redirect("/farmer/dashboard");
            }

            if (result.IsLockedOut)
                ErrorMessage = "Account locked. Try again in 10 minutes.";
            else
                ErrorMessage = "Invalid login attempt.";

            return Page();
        }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username or Email")]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me")]
            public bool RememberMe { get; set; }
        }
    }
}
