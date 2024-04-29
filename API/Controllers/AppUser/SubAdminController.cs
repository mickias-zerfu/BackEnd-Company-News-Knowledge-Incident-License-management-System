using Core.Entities.AppUser;
using Core.Interfaces.auth;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers.AppUser
{

    [ApiController]
    [Route("api/admin")]
    public class SubAdminController : ControllerBase
    {
        private readonly ISubAdminService _subAdminService;

        public SubAdminController(ISubAdminService subAdminService)
        {
            _subAdminService = subAdminService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserEntity user)
        {
            var result = await _subAdminService.AdminLoginAsync(user.UserName, user.Password);

            // if (result)
            // {
                return Ok(new { response = result});
            // }
            // else
            // {
            //     return BadRequest(new { message = result.Message });
            // }
        }
        [HttpPost("insert")]
        public async Task<IActionResult> InsertSubAdmin(SubAdmin subAdmin)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _subAdminService.InsertSubAdminAsync(subAdmin);
            if (result)
            {
                return Ok(new { message = "Inserted Successfully" });
            }
            else
            {
                return StatusCode(500, "Failed to insert subadmin");
            }
        }

        [HttpPost("getSubAdmin")]
        public async Task<IActionResult> GetSubAdmin(SubAdminQueryParameters queryParameters)
        {
            // Retrieve subadmins based on query parameters
            var subAdmins = await _subAdminService.GetSubAdminsAsync(queryParameters);

            // Check if subadmins were found
            if (subAdmins != null)
            {
                return Ok(subAdmins);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("getSingleSubAdmin")]
        public async Task<IActionResult> GetSingleSubAdmin(string request)
        {
            if (string.IsNullOrEmpty(request.ToString()))
            {
                return BadRequest("Please provide a valid user id.");
            }

            var subAdmin = await _subAdminService.GetSingleSubAdminAsync(request);
            if (subAdmin == null)
            {
                return NotFound("Subadmin not found.");
            }

            return Ok(subAdmin);
        }

        [HttpPut("updateSubAdmin")]
        public async Task<IActionResult> UpdateSubAdmin(SubAdmin subAdmin)
        {

            var result = await _subAdminService.UpdateSubAdminAsync(subAdmin);
            if (result)
                return Ok(new { message = "SubAdmin updated successfully" });
            else
                return BadRequest(new { message = "Failed to update SubAdmin" });
        }

        [HttpDelete("deleteSubAdmin")]
        public async Task<IActionResult> DeleteSubAdmin(int userId)
        {
            var result = await _subAdminService.DeleteSubAdminAsync(userId);
            if (result)
                return Ok(new { message = "SubAdmin deleted successfully" });
            else
                return BadRequest(new { message = "Failed to delete SubAdmin" });
        }
        [HttpPost("inactiveSubadmin")]
        public async Task<IActionResult> InactivateSubAdmin(int subAdminId)
        {
            var result = await _subAdminService.InactivateSubAdminAsync(subAdminId);
            if (result)
                return Ok(new { message = "Subadmin inactivated successfully" });
            else
                return BadRequest(new { message = "Failed to inactivate subadmin" });
        }

        [HttpPost("activeSubadmin")]
        public async Task<IActionResult> ActivateSubAdmin(int subAdminId)
        {
            var result = await _subAdminService.ActivateSubAdminAsync(subAdminId);
            if (result)
                return Ok(new { message = "Subadmin activated successfully" });
            else
                return BadRequest(new { message = "Failed to activate subadmin" });
        }
    }
}