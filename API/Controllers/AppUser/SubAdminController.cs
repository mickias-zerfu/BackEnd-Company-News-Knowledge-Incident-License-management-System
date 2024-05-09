using System.Security.Claims;
using API.Dtos;
using Core.Entities.AppUser;
using Core.Interfaces.auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers.AppUser
{

    [ApiController]
    [Route("api/admin")]
    public class SubAdminController : ControllerBase
    {
        private readonly ISubAdminService _subAdminService;
        private readonly UserManager<SubAdmin> _userManager;
        private readonly SignInManager<SubAdmin> _signInManager;
        private readonly ILogger<SubAdminController> _logger;

        public SubAdminController(UserManager<SubAdmin> userManager, SignInManager<SubAdmin> signInManager,
            ILogger<SubAdminController> logger, ISubAdminService subAdminService)
        {
            _subAdminService = subAdminService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }
        [HttpGet("testauth")]
        [Authorize]
        public ActionResult<string> GetSecretText()
        {
            return "secret stuff";
        }

        // [Authorize]
        [HttpGet("getSingleSubAdmin")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(email);

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = _subAdminService.CreateToken(user),
                Email = user.Email,
                Status = user.Status,
                RoleId = user.RoleId,
                Access = user.Access,
                Message = "Found User Successfully",
            };
        }

        // [Authorize]
        [HttpGet("getSubAdmin")]
        public async Task<ActionResult<List<UserDto>>> GetSubAdmin()
        {
            var subAdmins = await _userManager.Users.ToListAsync();
            if (subAdmins != null)
            {
                return Ok(subAdmins);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for email: {Email}", loginDto.Email);
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            // logger.LogInformation(user.DisplayName);
            if (user == null)
            {
                _logger.LogWarning("User not found for email: {Email}", loginDto.Email);
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Login failed for email: {Email}", loginDto.Email);
                return Unauthorized();
            }
            _logger.LogInformation("Login successful for email: {Email}", loginDto.Email);

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = _subAdminService.CreateToken(user),
                Email = user.Email,
                Status = user.Status,
                RoleId = user.RoleId,
                Access = user.Access,
                Message = "Login Successfully",
            };
        }



        // [Authorize]
        [HttpPost("insert")]
        public async Task<ActionResult<UserDto>> InsertSubAdmin(RegisterDto subAdmin)
        {
            if (CheckEmailExistsAsync(subAdmin.Email).Result.Value)
            {
                return new BadRequestObjectResult(
                    new { Errors = new[] { "Email address is in use" } });
            }
            _logger.LogInformation("Login attempt for email: {Email}", subAdmin.Email);
            var user = new SubAdmin
            {
                DisplayName = subAdmin.DisplayName,
                Email = subAdmin.Email,
                UserName = subAdmin.Email,
                RoleId = 1,
                Status = 1,
                Access = subAdmin.Access,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, subAdmin.Password);

            if (!result.Succeeded)
            {

                _logger.LogWarning("Login failed for email: {Email}", subAdmin.Email);
                return BadRequest(400);
            }

            _logger.LogInformation("Login successful for email: {Email}", subAdmin.Email);
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = _subAdminService.CreateToken(user),
                Email = user.Email,
                Status = user.Status,
                RoleId = user.RoleId,
                Access = user.Access,
                Message = "Account Created Successfully",
            };

        }

        // [Authorize]
        [HttpPut("updateSubAdmin")]
        public async Task<ActionResult<UserDto>> UpdateSubAdmin(RegisterDto subAdmin)
        {
            var user = await _userManager.FindByEmailAsync(subAdmin.Email);
            if (user == null) return NotFound();

            user.DisplayName = subAdmin.DisplayName;
            user.Email = subAdmin.Email;
            user.UserName = subAdmin.Email;
            user.Status = 1;
            user.Access = subAdmin.Access;
            user.Updated_at = DateTime.Now;
            //update other properties

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { message = "SubAdmin updated successfully" });
            }

            return BadRequest(new { message = "Failed to update SubAdmin" });
        }

        // [Authorize]
        [HttpPost("inactiveSubadmin")]
        public async Task<ActionResult<UserDto>> InactiveSubAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.Status = 0;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { message = "Subadmin inactivated successfully" });
            }

            return BadRequest(new { message = "Failed to inactivate subadmin" });
        }
        // [Authorize]
        [HttpPost("activeSubadmin")]
        public async Task<ActionResult<UserDto>> ActiveSubAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.Status = 1;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { message = "Subadmin activated successfully" });
            }

            return BadRequest(new { message = "Failed to activate subadmin" });
        }
        // [Authorize]
        [HttpDelete("deleteSubAdmin")]
        public async Task<IActionResult> DeleteSubAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { message = "Subadmin deleted successfully" });
            }
            else
            {
                return BadRequest(new { message = "Failed to delete subadmin" });
            }
        }
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}