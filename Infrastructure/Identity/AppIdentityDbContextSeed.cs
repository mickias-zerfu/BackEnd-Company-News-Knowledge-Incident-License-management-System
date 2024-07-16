using System.Diagnostics;
using Core.Entities.AppUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAndRolesAsync(UserManager<SubAdmin> userManager, RoleManager<IdentityRole> roleManager, ILogger logger)
        {
            logger.LogInformation("Seeding Roles and Users");

            // Seed Roles
            var roles = new[] { "Admin", "SubAdmin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    logger.LogInformation($"Creating role: {role}");
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            } 
            if (!userManager.Users.Any())
            {
                logger.LogInformation("Creating new user...");

                var user = new SubAdmin
                {
                    DisplayName = "Super_Admin",
                    UserName = "superadmin",
                    Email = "superadmin@gmail.com",
                    SpecificRoleId = 2,
                    Status = 1,
                    Access = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                    Created_at = DateTime.Now,
                    Updated_at = DateTime.Now
                };
                 
                var result = await userManager.CreateAsync(user, "P@ssw0rdIB");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                    logger.LogInformation("User created and assigned role successfully.");
                }
                else
                {
                    logger.LogError("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
                 
            }
        }
    }
}
