using RUYA_API.Application.Services.Admin.DTOs.Dashboard;

namespace RUYA_API.Application.Services.Admin.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync();

        Task<IEnumerable<ChartPointDto>> GetUsersGrowthAsync(int days = 30);

        Task<IEnumerable<ChartPointDto>> GetToursGrowthAsync(int days = 30);

        Task<IEnumerable<ChartPointDto>> GetTourStatusDistributionAsync();

        Task<IEnumerable<ChartPointDto>> GetArtifactsByCategoryAsync();

        Task<IEnumerable<ChartPointDto>> GetArtifactsByCivilizationAsync();

        Task<IEnumerable<ChartPointDto>> GetTopSitesAsync(int count = 5);

        Task<IEnumerable<ChartPointDto>> GetTopArtifactsAsync(int count = 5);
    }
}
