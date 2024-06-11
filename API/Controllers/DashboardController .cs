
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Authorize(Roles = "Admin,SubAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController :ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("counts")]
        public async Task<ActionResult<DashboardCounts>> GetDashboardCounts()
        {
            var counts = await _dashboardService.GetDashboardCounts();
            return Ok(counts);
        }

        [HttpGet("monthly-uploads")]
        public async Task<ActionResult<List<MonthlyUploads>>> GetMonthlyUploads()
        {
            var monthlyUploads = await _dashboardService.GetMonthlyUploads();
            return Ok(monthlyUploads);
        }
    }
}