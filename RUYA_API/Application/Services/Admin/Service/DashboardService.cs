using Microsoft.EntityFrameworkCore;
using RUYA_API.Application.Services.Admin.DTOs.Dashboard;
using RUYA_API.Application.Services.Admin.Interfaces;
using RUYA_API.Domain.Enums;
using RUYA_API.Infrastructure.Context;

namespace RUYA_API.Application.Services.Admin.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly RuyaContext _context;

        public DashboardService(RuyaContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var totalUsers = await _context.Users.AsNoTracking().CountAsync();
            var totalSites = await _context.Sites.AsNoTracking().CountAsync();
            var totalArtifacts = await _context.Artifacts.AsNoTracking().CountAsync();
            var totalTours = await _context.Tours.AsNoTracking().CountAsync();
            var totalConversations = await _context.Conversations.AsNoTracking().CountAsync();
            var totalMessages = await _context.Messages.AsNoTracking().CountAsync();

            return new DashboardStatsDto
            {
                TotalUsers = totalUsers,
                TotalSites = totalSites,
                TotalArtifacts = totalArtifacts,
                TotalTours = totalTours,
                TotalConversations = totalConversations,
                TotalMessages = totalMessages
            };
        }

        public async Task<IEnumerable<ChartPointDto>> GetUsersGrowthAsync(int days = 30)
        {
            // The User entity does not track CreatedAt in this project.
            // As a meaningful proxy for "new active users", we compute
            // the number of users whose first Tour was created on each day.

            var end = DateTime.UtcNow.Date;
            var start = end.AddDays(-days + 1);

            // For each user, get the first tour created date
            var firstTourDates = await _context.Tours
                .AsNoTracking()
                .GroupBy(t => t.UserId)
                .Select(g => new { UserId = g.Key, First = g.Min(t => t.CreatedAt) })
                .Where(x => x.First >= start && x.First <= end.AddDays(1))
                .ToListAsync();

            var counts = firstTourDates
                .GroupBy(x => x.First.Date)
                .ToDictionary(g => g.Key, g => (long)g.Count());

            var result = new List<ChartPointDto>();

            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                counts.TryGetValue(dt, out var v);
                result.Add(new ChartPointDto { Label = dt.ToString("yyyy-MM-dd"), Value = v });
            }

            return result;
        }

        public async Task<IEnumerable<ChartPointDto>> GetToursGrowthAsync(int days = 30)
        {
            var end = DateTime.UtcNow.Date;
            var start = end.AddDays(-days + 1);

            var tourCounts = await _context.Tours
                .AsNoTracking()
                .Where(t => t.CreatedAt >= start && t.CreatedAt < end.AddDays(1))
                .GroupBy(t => t.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

            var dict = tourCounts.ToDictionary(x => x.Date, x => (long)x.Count);

            var result = new List<ChartPointDto>();

            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                dict.TryGetValue(dt, out var v);
                result.Add(new ChartPointDto { Label = dt.ToString("yyyy-MM-dd"), Value = v });
            }

            return result;
        }

        public async Task<IEnumerable<ChartPointDto>> GetTourStatusDistributionAsync()
        {
            var data = await _context.Tours
                .AsNoTracking()
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = data.Select(d => new ChartPointDto
            {
                Label = Enum.GetName(typeof(TourStatus), d.Status) ?? d.Status.ToString(),
                Value = d.Count
            })
            .OrderByDescending(x => x.Value)
            .ToList();

            return result;
        }

        public async Task<IEnumerable<ChartPointDto>> GetArtifactsByCategoryAsync()
        {
            var data = await _context.Artifacts
                .AsNoTracking()
                .GroupBy(a => a.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = data
                .Select(d => new ChartPointDto { Label = string.IsNullOrEmpty(d.Category) ? "Unknown" : d.Category, Value = d.Count })
                .OrderByDescending(x => x.Value)
                .ToList();

            return result;
        }

        public async Task<IEnumerable<ChartPointDto>> GetArtifactsByCivilizationAsync()
        {
            var data = await _context.Artifacts
                .AsNoTracking()
                .GroupBy(a => a.Civilization)
                .Select(g => new { Civilization = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = data
                .Select(d => new ChartPointDto { Label = string.IsNullOrEmpty(d.Civilization) ? "Unknown" : d.Civilization, Value = d.Count })
                .OrderByDescending(x => x.Value)
                .ToList();

            return result;
        }

        public async Task<IEnumerable<ChartPointDto>> GetTopSitesAsync(int count = 5)
        {
            var data = await _context.Sites
                .AsNoTracking()
                .Select(s => new { s.Name, Tours = s.Tours.Count })
                .OrderByDescending(x => x.Tours)
                .Take(count)
                .ToListAsync();

            return data.Select(d => new ChartPointDto { Label = d.Name, Value = d.Tours }).ToList();
        }

        public async Task<IEnumerable<ChartPointDto>> GetTopArtifactsAsync(int count = 5)
        {
            // Use TourStops (visits) as the metric for artifact popularity
            var top = await _context.TourStops
                .AsNoTracking()
                .GroupBy(ts => ts.ArtifactId)
                .Select(g => new { ArtifactId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(count)
                .ToListAsync();

            if (!top.Any())
                return new List<ChartPointDto>();

            var artifactIds = top.Select(t => t.ArtifactId).ToList();

            var artifacts = await _context.Artifacts
                .AsNoTracking()
                .Where(a => artifactIds.Contains(a.Id))
                .ToDictionaryAsync(a => a.Id, a => a.Name);

            var result = top.Select(t => new ChartPointDto
            {
                Label = artifacts.TryGetValue(t.ArtifactId, out var name) ? name : t.ArtifactId.ToString(),
                Value = t.Count
            }).ToList();

            return result;
        }
    }
}
