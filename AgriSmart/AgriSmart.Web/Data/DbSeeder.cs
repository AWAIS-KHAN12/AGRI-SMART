using System;
using System.Threading.Tasks;
using AgriSmart.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AgriSmart.Web.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

            if (Startup.UseSqlServer)
            {
                try { await context.Database.MigrateAsync(); }
                catch { await context.Database.EnsureCreatedAsync(); }
            }
            else
            {
                await context.Database.EnsureCreatedAsync();
            }

            foreach (var role in new[] { "Admin", "Farmer" })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
            }

            var admin = await userManager.FindByNameAsync("admin");
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@agrismart.pk",
                    FullName = "System Administrator",
                    Region = "Islamabad",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                var result = await userManager.CreateAsync(admin, "Admin@1234");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }
            else
            {
                admin.IsActive = true;
                admin.PasswordHash = userManager.PasswordHasher.HashPassword(admin, "Admin@1234");
                var result = await userManager.UpdateAsync(admin);
                if (!result.Succeeded)
                {
                    logger.LogError("Failed to update seeded admin user: {Errors}", string.Join(", ", result.Errors));
                }
                if (!await userManager.IsInRoleAsync(admin, "Admin"))
                    await userManager.AddToRoleAsync(admin, "Admin");
            }

            // ── Seed default farmer account ──────────────────────────────────────
            var farmer = await userManager.FindByNameAsync("farmer");
            if (farmer == null)
            {
                farmer = new ApplicationUser
                {
                    UserName = "farmer",
                    Email = "farmer@agrismart.pk",
                    FullName = "Demo Farmer",
                    Region = "Punjab",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                var farmerResult = await userManager.CreateAsync(farmer, "Farmer@1234");
                if (farmerResult.Succeeded)
                    await userManager.AddToRoleAsync(farmer, "Farmer");
            }
            else
            {
                farmer.IsActive = true;
                farmer.PasswordHash = userManager.PasswordHasher.HashPassword(farmer, "Farmer@1234");
                await userManager.UpdateAsync(farmer);
                if (!await userManager.IsInRoleAsync(farmer, "Farmer"))
                    await userManager.AddToRoleAsync(farmer, "Farmer");
            }

            logger.LogInformation("Database seeding completed ({Provider}).",
                Startup.UseSqlServer ? "SQL Server" : "SQLite");
        }
    }
}
