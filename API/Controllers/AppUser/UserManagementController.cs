using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.AppUser;
using Core.Interfaces.auth;
using Infrastructure.Data.Auth;
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
        [HttpPost("login")]
        public async Task<ActionResult<bool>> LoginUser(LoginUserEntity user)
        {
            return await _authService.IsValidUser(user.UserName, user.Password);
        }
    }
}