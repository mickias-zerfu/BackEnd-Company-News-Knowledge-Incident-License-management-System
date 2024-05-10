using API.Dtos;
using Core.Entities.AppUser;
using Core.Interfaces.auth;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;

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
    }
}