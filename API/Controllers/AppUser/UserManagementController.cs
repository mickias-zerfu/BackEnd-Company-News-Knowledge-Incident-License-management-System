using API.Dtos;
using Core.Entities.AppUser;
using Core.Interfaces.auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using System.Security.Claims;

namespace API.Controllers.AppUser
{
    [ApiController]
    [Route("api/user")]
    public class UserManagementController : ControllerBase
    {
        private readonly IActiveDirectoryService _authService;

        private readonly ISubAdminService _subAdminService;
        public UserManagementController(IActiveDirectoryService authService, ISubAdminService subAdminService)
        {
            _authService = authService;
            _subAdminService = subAdminService;
        }
        // [HttpPost("login")]
        // public async Task<ActionResult<bool>> LoginUser(LoginUserEntity user)
        // {
        //     return await _authService.IsValidUser(user.UserName, user.Password);
        // }
        [HttpPost("domainlogin")]
        public async Task<ActionResult<DomainDto>> LoginUser(LoginUserEntity user)
        {
            var userData = await _authService.IsValidUser(user.UserName, user.Password);
            if (userData != null)
            {
                return Ok(userData);
            }
            else
            {
                return Unauthorized(); // Or any other appropriate status code
            }
        }

        [HttpGet("getDomainUser")]
        public async Task<ActionResult<DomainDto>> GetLoginUser()
        { 
            var name = HttpContext.User?.Identity?.Name;
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Username not found in the claims");
            }
            var result = await _authService.GetCurrentUser(name);
            if (result.Status == 1)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }

        }
    }
}
 