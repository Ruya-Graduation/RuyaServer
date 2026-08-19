namespace RUYA_API.Application.Services.Admin.DTOs.Dashboard
{
    public class DashboardStatsDto
    {
        public long TotalUsers { get; set; }

        public long TotalSites { get; set; }

        public long TotalArtifacts { get; set; }

        public long TotalTours { get; set; }

        public long TotalConversations { get; set; }

        public long TotalMessages { get; set; }
    }
}
