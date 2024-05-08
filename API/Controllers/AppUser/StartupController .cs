using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Identity;
using Core.Entities.AppUser; // Import your User model

[ApiController]
[Route("api/[controller]")]
public class StartupController : ControllerBase
{
    private readonly ILogger<StartupController> _logger;
    private readonly AppIdentityDbContext _context;

    public StartupController(ILogger<StartupController> logger, AppIdentityDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost("start")]
    public async Task<IActionResult> Post()
    {
        
        try
        {
            // Check if there are any super admins already
            // var superAdminExists = await _context.SubAdmins.AnyAsync(u => u.RoleId == 2);

            // if (!superAdminExists)
            // {
                // Create the super admin user
                var user = new SubAdmin
                {
                    DisplayName = "Super Admin",
                    Email = "superadmin@gmail.com",
                    PasswordHash = "P@ssw0rdMZ",
                    RoleId = 2,
                    Status = 1,
                    Access = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
                };

                // Add the user to the database
                // _context.SubAdmins.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new { status = 1, message = "Super Admin Created Successfully", data = user });
            // }

            // return Ok(new { status = 0, message = "Super Admin already exists" });
        }
        catch (Exception ex)
        {
            // Log the error
            _logger.LogError(ex, "Error creating super admin");
            return StatusCode(500, "Internal server error");
        }
    }
}
