using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AgriSmart.Web.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(100)]
        public string Region { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}
