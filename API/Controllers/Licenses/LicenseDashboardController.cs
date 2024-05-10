
using Core.Entities.licenseEntity;
using Core.Interfaces.licenses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Licenses
{
    [ApiController]
    [Route("api/licenses")]
    public class LicenseDashboardController : ControllerBase
    {
        private readonly ILicenseDashboardService _dashboardService;

        public LicenseDashboardController(ILicenseDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("counts")]
        public async Task<ActionResult<LicenseDashboardCount>> GetDashboardCounts()
        {
            var counts = await _dashboardService.GetLicenseDashboardCounts();
            return Ok(counts);
        }
    }
}