
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DashboardService : IDashboardService
    {
        private readonly StoreContext _dbContext;

        public DashboardService(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DashboardCounts> GetDashboardCounts()
        {
            // Query the database to get counts for each entity type
            // Example:
            var counts = new DashboardCounts
            {
                NewsCount = await _dbContext.News.CountAsync(),
                IncidentCount = await _dbContext.Incidents.CountAsync(),
                SharedResourceCount = await _dbContext.SharedResources.CountAsync(),
                KnowledgeBaseCount = await _dbContext.KnowledgeBases.CountAsync()
            };

            return counts;
        }

        public async Task<List<MonthlyUploads>> GetMonthlyUploads()
        {
            // Query the database to get monthly upload data for each entity type
            // Example:
            var monthlyUploads = await _dbContext.News
                .GroupBy(n => n.Created_at)
                .Select(g => new MonthlyUploads
                {
                    // Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    NewsUploads = g.Count()
                    // Similar queries for other entity types
                })
                .ToListAsync();

            return monthlyUploads;
        }
    }
}