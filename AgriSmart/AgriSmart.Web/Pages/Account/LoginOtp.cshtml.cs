using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AgriSmart.Web.Models;
using AgriSmart.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace AgriSmart.Web.Pages.Account
{
    public class LoginOtpModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EmailService _emailService;
        private readonly IMemoryCache _cache;

        public LoginOtpModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            EmailService emailService,
            IMemoryCache cache)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _cache = cache;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string OtpCode { get; set; }

        [BindProperty]
        public int Step { get; set; } = 1;

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string DemoOtpCode { get; set; }

        public void OnGet(string email = null)
        {
            if (!string.IsNullOrEmpty(email))
            {
                Email = email;
                Step = 2;
                if (_emailService.IsDummy())
                {
                    var cacheKey = $"OTP_{Email.ToLower().Trim()}";
                    if (_cache.TryGetValue(cacheKey, out string savedOtp))
                    {
                        DemoOtpCode = savedOtp;
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Step == 2 && _emailService.IsDummy() && !string.IsNullOrEmpty(Email))
            {
                var cacheKey = $"OTP_{Email.ToLower().Trim()}";
                if (_cache.TryGetValue(cacheKey, out string savedOtp))
                {
                    DemoOtpCode = savedOtp;
                }
            }

            if (Step == 1)
            {
                if (string.IsNullOrWhiteSpace(Email) || !new EmailAddressAttribute().IsValid(Email))
                {
                    ErrorMessage = "Please enter a valid email address.";
                    return Page();
                }

                // Generate OTP
                var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
                
                // Store OTP in cache for 5 minutes
                var cacheKey = $"OTP_{Email.ToLower().Trim()}";
                _cache.Set(cacheKey, otp, TimeSpan.FromMinutes(5));

                // Send email
                var sent = await _emailService.SendOtpEmailAsync(Email, otp);
                if (sent)
                {
                    if (_emailService.IsDummy())
                    {
                        DemoOtpCode = otp;
                        SuccessMessage = "A verification code has been generated. Since SMTP is not configured, please use the code below:";
                    }
                    else
                    {
                        SuccessMessage = "A 6-digit verification code has been sent to your email.";
                    }
                    Step = 2;
                }
                else
                {
                    ErrorMessage = "Failed to send verification email. Please check your SMTP settings or try again later.";
                }

                return Page();
            }
            else if (Step == 2)
            {
                if (string.IsNullOrWhiteSpace(Email))
                {
                    ErrorMessage = "Session expired. Please try again.";
                    Step = 1;
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(OtpCode) || OtpCode.Length != 6)
                {
                    ErrorMessage = "Please enter a valid 6-digit verification code.";
                    return Page();
                }

                var cacheKey = $"OTP_{Email.ToLower().Trim()}";
                if (_cache.TryGetValue(cacheKey, out string savedOtp))
                {
                    if (savedOtp == OtpCode.Trim())
                    {
                        // Remove from cache once used
                        _cache.Remove(cacheKey);

                        // Find or create user
                        var user = await _userManager.FindByEmailAsync(Email);
                        if (user == null)
                        {
                            // Create a new user automatically!
                            var baseUsername = Email.Split('@')[0].Replace(".", "").Replace("-", "");
                            var username = baseUsername;
                            int count = 1;
                            while (await _userManager.FindByNameAsync(username) != null)
                            {
                                username = $"{baseUsername}{count++}";
                            }

                            user = new ApplicationUser
                            {
                                UserName = username,
                                Email = Email,
                                FullName = baseUsername,
                                Region = "Punjab",
                                EmailConfirmed = true,
                                IsActive = true,
                                CreatedAt = DateTime.UtcNow
                            };

                            var createResult = await _userManager.CreateAsync(user, Guid.NewGuid().ToString() + "A!");
                            if (!createResult.Succeeded)
                            {
                                ErrorMessage = "Failed to register your user account automatically.";
                                return Page();
                            }
                            await _userManager.AddToRoleAsync(user, "Farmer");
                        }

                        if (!user.IsActive)
                        {
                            ErrorMessage = "Your account has been deactivated. Please contact support.";
                            return Page();
                        }

                        // Sign in the user
                        await _signInManager.SignInAsync(user, isPersistent: true);

                        if (await _userManager.IsInRoleAsync(user, "Admin"))
                            return Redirect("/admin/panel");

                        return Redirect("/farmer/dashboard");
                    }
                }

                ErrorMessage = "Invalid or expired verification code. Please check and try again.";
                return Page();
            }

            return Page();
        }
    }
}
