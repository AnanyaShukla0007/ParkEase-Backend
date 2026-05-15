using Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure.Seed;

public static class AuthUserSeeder
{
    private const string AdminEmail = "admin@parkease.in";
    private const string AdminPassword = "Admin@ParkEase2026";

    public static async Task SeedAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

        if (!await roleManager.RoleExistsAsync("ADMIN"))
            await roleManager.CreateAsync(new IdentityRole<int>("ADMIN"));

        var admin = await userManager.FindByEmailAsync(AdminEmail);
        if (admin is null)
        {
            admin = new User
            {
                FullName = "ParkEase Admin",
                Email = AdminEmail,
                UserName = AdminEmail,
                Role = "ADMIN",
                IsActive = true,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var created = await userManager.CreateAsync(admin, AdminPassword);
            if (!created.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", created.Errors.Select(x => x.Description)));
            }
        }
        else
        {
            admin.FullName = string.IsNullOrWhiteSpace(admin.FullName)
                ? "ParkEase Admin"
                : admin.FullName;
            admin.UserName = AdminEmail;
            admin.Email = AdminEmail;
            admin.Role = "ADMIN";
            admin.IsActive = true;
            admin.EmailConfirmed = true;
            admin.UpdatedAt = DateTime.UtcNow;

            var updated = await userManager.UpdateAsync(admin);
            if (!updated.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", updated.Errors.Select(x => x.Description)));
            }

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(admin);
            var reset = await userManager.ResetPasswordAsync(admin, resetToken, AdminPassword);
            if (!reset.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", reset.Errors.Select(x => x.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(admin, "ADMIN"))
            await userManager.AddToRoleAsync(admin, "ADMIN");
    }
}
