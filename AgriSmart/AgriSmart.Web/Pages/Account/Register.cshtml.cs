using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AgriSmart.Web.Helpers;
using AgriSmart.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AgriSmart.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel { Role = "Farmer", AcceptTerms = true };

        public string ErrorMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!Input.AcceptTerms)
            {
                ErrorMessage = "Please accept the terms.";
                return Page();
            }

            if (Input.Password != Input.ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            var user = new ApplicationUser
            {
                UserName = Input.Username,
                Email = Input.Email,
                FullName = Input.FullName,
                Region = Input.Region,
                PhoneNumber = Input.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (!result.Succeeded)
            {
                ErrorMessage = string.Join(" ", result.Errors.Select(e =>
                    e.Code.Contains("Duplicate") ? "This username or email already exists." : e.Description));
                return Page();
            }

            await _userManager.AddToRoleAsync(user, "Farmer");
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Redirect("/farmer/dashboard");
        }

        public class InputModel
        {
            [Required]
            public string FullName { get; set; }

            [Required]
            public string Region { get; set; }

            public string Role { get; set; }

            [Required]
            public string Username { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, MinLength(8)]
            public string Password { get; set; }

            [Required]
            public string ConfirmPassword { get; set; }

            public string PhoneNumber { get; set; }

            public bool AcceptTerms { get; set; }
        }
    }
}
