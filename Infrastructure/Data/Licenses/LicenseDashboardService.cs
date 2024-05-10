
using Core.Entities.licenseEntity;
using Core.Interfaces.licenses;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Licenses
{
    public class LicenseDashboardService : ILicenseDashboardService
    {
        private readonly StoreContext _dbContext;

        public LicenseDashboardService(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LicenseDashboardCount> GetLicenseDashboardCounts()
        {
            // Query the database to get counts for each entity type
            // Example:
            var counts = new LicenseDashboardCount
            {
                LicenseCount = await _dbContext.Licenses.CountAsync(),
                LicenseManagerCount = await _dbContext.LicenseManagers.CountAsync(),
                SoftwareCount = await _dbContext.SoftwareProducts.CountAsync(),
                ExpiredCount = await GetExpiredLicenseCountAsync()
            };

            return counts;
        }
        public async Task<int> GetExpiredLicenseCountAsync()
        {
            var today = DateTime.Today;

            return await _dbContext.Licenses
              .Where(l => l.ExpirationDate < today)
              .CountAsync();
        }
    }
}