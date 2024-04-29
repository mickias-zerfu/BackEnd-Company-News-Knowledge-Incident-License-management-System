using Core.Entities.AppUser;
using Core.Interfaces.auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AppUser
{
    [ApiController]
    [Route("api/user")]
    public class UserManagementController : ControllerBase
    {
        private readonly IActiveDirectoryService _authService;

        public UserManagementController(IActiveDirectoryService authService)
        {
            _authService = authService;
        }
        // [HttpPost("login")]
        // public async Task<ActionResult<bool>> LoginUser(LoginUserEntity user)
        // {
        //     return await _authService.IsValidUser(user.UserName, user.Password);
        // }
        [HttpPost("login")]
        public async Task<ActionResult<object>> LoginUser(LoginUserEntity user)
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