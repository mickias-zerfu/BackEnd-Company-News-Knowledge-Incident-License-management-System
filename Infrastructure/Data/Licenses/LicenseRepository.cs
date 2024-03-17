using Core.Entities.licenseEntity;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data.Licenses
{
    public class LicenseRepository : ILicenseRepository
    {
        private readonly StoreContext _context;

        public LicenseRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<License> GetLicenseByIdAsync(int id)
        {
            return await _context.Licenses.FindAsync(id);
        }

        public async Task<IReadOnlyList<License>> GetAllLicensesAsync()
        {
            return await _context.Licenses.ToListAsync();
        }

        public async Task<License> CreateLicenseAsync(License license)
        {
            _context.Licenses.Add(license);
            await _context.SaveChangesAsync();
            return license;
        }

        public async Task<License> UpdateLicenseAsync(License license)
        {
            _context.Entry(license).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return license;
        }

        public async Task DeleteLicenseAsync(int id)
        {
            var licenseToDelete = await _context.Licenses.FindAsync(id);
            if (licenseToDelete != null)
            {
                _context.Licenses.Remove(licenseToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
