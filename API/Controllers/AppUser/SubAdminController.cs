using System.Security.Claims;
using API.Dtos;
using Core.Entities.AppUser;
using Core.Interfaces.auth;
using Infrastructure.Data.Auth;
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
        private readonly UserRoleService _userRoleService;

        public SubAdminController(UserManager<SubAdmin> userManager, SignInManager<SubAdmin> signInManager,
            ILogger<SubAdminController> logger, ISubAdminService subAdminService, UserRoleService userRoleService)
        {
            _subAdminService = subAdminService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _userRoleService = userRoleService;
        }
        [HttpGet("testauth")]
        [Authorize]
        public ActionResult<string> GetSecretText()
        {
            return "secret stuff";
        }
         
        [Authorize]
        [HttpGet("getSingleSubAdmin")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return null;
            }
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = await _subAdminService.CreateToken(user),
                Email = user.Email,
                Status = user.Status,
                RoleId = user.SpecificRoleId,
                Access = user.Access,
                Message = "Found User Successfully",
            };
        }

        //[Authorize]
        [HttpPost("subAdminById")]
        public async Task<ActionResult<UserDto>> getSingleSubAdmin([FromBody] StatusUpdateRequest request)
        {
            _logger.LogInformation("Login attempt for user", request.Id);
            var user = await _userManager.FindByIdAsync(request.Id);
            _logger.LogInformation("Login attempt for user rer", user);
            if (user == null)
            {
                return null;
            }
            return new UserDto
            {
                DisplayName = user.DisplayName, 
                Email = user.Email,
                Status = user.Status,
                RoleId = user.SpecificRoleId,
                Access = user.Access,
                Message = "Found User Successfully",
            };
        }

        [Authorize(Roles = "Admin")]
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
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            if (result.Succeeded && user.Status == 0)
            {
                return Unauthorized(new { Message = "Your account has been deactivated. Please contact the administrator." });
            }

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = await _subAdminService.CreateToken(user),
                Email = user.Email,
                Status = user.Status,
                RoleId = user.SpecificRoleId,
                Access = user.Access,
                Message = "Login Successfully",
            };
        }
        //[HttpPost("login")]
        //public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        //{ 
        //    var user = await _userManager.FindByEmailAsync(loginDto.Email); 
        //    if (user == null)
        //    { 
        //        return Unauthorized();
        //    }

        //    var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        //    if (!result.Succeeded)
        //    {
        //        return Unauthorized();
        //    }
        //    if (result.Succeeded && user.Status==0)
        //    {
        //        return Unauthorized();
        //    }

        //    return new UserDto
        //    {
        //        DisplayName = user.DisplayName,
        //        Token = _subAdminService.CreateToken(user),
        //        Email = user.Email,
        //        Status = user.Status,
        //        RoleId = user.RoleId,
        //        Access = user.Access,
        //        Message = "Login Successfully",
        //    };
        //}



        [Authorize(Roles = "Admin,SubAdmin,user")]
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
                SpecificRoleId = 1,
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

            // Assign the "SubAdmin" role to the newly created user
            var roleAssignmentResult = await _userRoleService.AssignRoleToUserAsync(user.Id, "SubAdmin");

            if (!roleAssignmentResult)
            {
                _logger.LogWarning("Role assignment failed for email: {Email}", subAdmin.Email);
                return BadRequest(new { Errors = new[] { "Failed to assign role to the user" } });
            }

                _logger.LogInformation("Login successful for email: {Email}", subAdmin.Email);
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = await _subAdminService.CreateToken(user),
                Email = user.Email,
                Status = user.Status,
                RoleId = user.SpecificRoleId,
                Access = user.Access,
                Message = "Account Created Successfully",
            };

        }


        [Authorize(Roles = "Admin")]
        [HttpPut("updateSubAdmin/{id}")]
        public async Task<ActionResult<UserDto>> UpdateSubAdmin(string id, RegisterDto subAdmin)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (CheckEmailExistsAsync(subAdmin.Email).Result.Value && user.Email != subAdmin.Email)
            {
                return new BadRequestObjectResult(
                    new { Errors = new[] { "Email address is in use" } });
            }
            if (user == null) return NotFound();
            
            user.DisplayName = subAdmin.DisplayName;
            user.Email = subAdmin.Email;
            user.UserName = subAdmin.Email; 
            var passwordHasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, subAdmin.Password); 
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

        [Authorize(Roles = "Admin")]
        [HttpPost("inactiveSubadmin")]
        public async Task<ActionResult<UserDto>> InactiveSubAdmin([FromBody] StatusUpdateRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null) return NotFound();

            user.Status = 0;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { message = "Subadmin inactivated successfully" });
            }

            return BadRequest(new { message = "Failed to inactivate subadmin" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("activeSubadmin")]
        public async Task<ActionResult<UserDto>> ActiveSubAdmin([FromBody] StatusUpdateRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null) return NotFound();

            user.Status = 1;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { message = "Subadmin activated successfully" });
            }

            return BadRequest(new { message = "Failed to activate subadmin" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("deleteSubAdmin")]
        public async Task<IActionResult> DeleteSubAdmin([FromBody] StatusUpdateRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

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