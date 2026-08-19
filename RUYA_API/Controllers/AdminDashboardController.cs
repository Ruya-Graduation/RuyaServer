using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RUYA_API.Application.Services.Admin.Interfaces;
using RUYA_API.Responses;

namespace RUYA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public AdminDashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _dashboardService.GetDashboardStatsAsync();
            return Ok(ResponseFactory.Success(stats, "Dashboard stats retrieved successfully."));
        }

        [HttpGet("users-growth")]
        public async Task<IActionResult> GetUsersGrowth([FromQuery] int days = 30)
        {
            var data = await _dashboardService.GetUsersGrowthAsync(days);
            return Ok(ResponseFactory.Success(data, "Users growth retrieved successfully."));
        }

        [HttpGet("tours-growth")]
        public async Task<IActionResult> GetToursGrowth([FromQuery] int days = 30)
        {
            var data = await _dashboardService.GetToursGrowthAsync(days);
            return Ok(ResponseFactory.Success(data, "Tours growth retrieved successfully."));
        }

        [HttpGet("tour-status-distribution")]
        public async Task<IActionResult> GetTourStatusDistribution()
        {
            var data = await _dashboardService.GetTourStatusDistributionAsync();
            return Ok(ResponseFactory.Success(data, "Tour status distribution retrieved successfully."));
        }

        [HttpGet("artifacts-by-category")]
        public async Task<IActionResult> GetArtifactsByCategory()
        {
            var data = await _dashboardService.GetArtifactsByCategoryAsync();
            return Ok(ResponseFactory.Success(data, "Artifacts by category retrieved successfully."));
        }

        [HttpGet("artifacts-by-civilization")]
        public async Task<IActionResult> GetArtifactsByCivilization()
        {
            var data = await _dashboardService.GetArtifactsByCivilizationAsync();
            return Ok(ResponseFactory.Success(data, "Artifacts by civilization retrieved successfully."));
        }

        [HttpGet("top-sites")]
        public async Task<IActionResult> GetTopSites([FromQuery] int count = 5)
        {
            var data = await _dashboardService.GetTopSitesAsync(count);
            return Ok(ResponseFactory.Success(data, "Top sites retrieved successfully."));
        }

        [HttpGet("top-artifacts")]
        public async Task<IActionResult> GetTopArtifacts([FromQuery] int count = 5)
        {
            var data = await _dashboardService.GetTopArtifactsAsync(count);
            return Ok(ResponseFactory.Success(data, "Top artifacts retrieved successfully."));
        }
    }
}
