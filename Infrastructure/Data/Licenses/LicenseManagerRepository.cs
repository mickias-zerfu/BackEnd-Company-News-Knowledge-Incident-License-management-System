
using System.Threading.Tasks;
using Core.Entities.licenseEntity;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Licenses
{
public class LicenseManagerRepository : ILicenseManagerRepository
    {
        private readonly StoreContext _context;

        public LicenseManagerRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<LicenseManager> GetLicenseManagerByIdAsync(int id)
        {
            return await _context.LicenseManagers.FindAsync(id);
        }

        public async Task<IReadOnlyList<LicenseManager>> GetAllLicenseManagersAsync()
        {
            return await _context.LicenseManagers.ToListAsync();
        }

        public async Task<LicenseManager> CreateLicenseManagerAsync(LicenseManager licenseManager)
        {
            _context.LicenseManagers.Add(licenseManager);
            await _context.SaveChangesAsync();
            return licenseManager;
        }

        public async Task<LicenseManager> UpdateLicenseManagerAsync(LicenseManager licenseManager)
        {
            _context.Entry(licenseManager).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return licenseManager;
        }

        public async Task DeleteLicenseManagerAsync(int id)
        {
            var licenseManagerToDelete = await _context.LicenseManagers.FindAsync(id);
            if (licenseManagerToDelete != null)
            {
                _context.LicenseManagers.Remove(licenseManagerToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}