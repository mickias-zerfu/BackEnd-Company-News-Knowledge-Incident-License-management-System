using System.Diagnostics;
using Core.Entities.AppUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<SubAdmin> userManager, ILogger logger)
        {
            logger.LogInformation("Seeding Users");
            if (!userManager.Users.Any())
            {
                logger.LogInformation("Creating new user...");

                var user = new SubAdmin
                {
                    DisplayName = "Super Admin",
                    UserName = "superadmin",
                    Email = "superadmin@gmail.com",
                    RoleId = 2,
                    Status = 1,
                    Access = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                    Created_at = DateTime.Now,
                    Updated_at = DateTime.Now
                };

                await userManager.CreateAsync(user, "P@ssw0rdMZ");

                logger.LogInformation("User created successfully.");
            }
        }
    }
}
