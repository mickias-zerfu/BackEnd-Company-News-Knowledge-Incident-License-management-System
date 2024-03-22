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

        public async Task<LicenseDto> GetLicenseByIdAsync(int id)
        {
            var l = await _context.Licenses
       .Include(l => l.LicenseManagers)
           .ThenInclude(lm => lm.Licenses) // Include LicenseManager entity
       .FirstOrDefaultAsync(l => l.Id == id);
            // Map License entity to LicenseDto
            var licenseDto = new LicenseDto
            {
                Id = l.Id,
                IssuedTo = l.IssuedTo,
                IssuedBy = l.IssuedBy,
                CreationDate = l.CreationDate,
                ActivationDate = l.ActivationDate,
                ExpirationDate = l.ExpirationDate,
                MaxUsers = l.MaxUsers,
                Activated = l.Activated,
                LicenseType = l.LicenseType,
                Notes = l.Notes,
                SoftwareProductId = l.SoftwareProductId,
                SoftwareProduct = _context.SoftwareProducts.FirstOrDefault(sp => sp.Id == l.SoftwareProductId),
                AssignedManagers = l.LicenseManagers.ToList()
            };
            // if (license == null)
            //     return null;
            // return await _context.Licenses
            //     .Where(l => l.Id == id)
            //     .Select(l => new LicenseDto
            //     {
            //     })
            //     .FirstOrDefaultAsync();
            return licenseDto;
        }

        public async Task<IReadOnlyList<LicenseDto>> GetAllLicensesAsync()
        {
            return await _context.Licenses
                .Select(l => new LicenseDto
                {
                    Id = l.Id,
                    IssuedTo = l.IssuedTo,
                    IssuedBy = l.IssuedBy,
                    CreationDate = l.CreationDate,
                    ActivationDate = l.ActivationDate,
                    ExpirationDate = l.ExpirationDate,
                    MaxUsers = l.MaxUsers,
                    Activated = l.Activated,
                    LicenseType = l.LicenseType,
                    Notes = l.Notes,
                    SoftwareProductId = l.SoftwareProductId,
                    SoftwareProduct = _context.SoftwareProducts.FirstOrDefault(sp => sp.Id == l.SoftwareProductId)
                })
                .ToListAsync();
        }
        // public async Task<License> CreateLicenseAsync(License license)
        // {
        //     _context.Licenses.Add(license);
        //     await _context.SaveChangesAsync();
        //     return license;
        // }
        public async Task<License> CreateLicenseAsync(License license)
        {
            // Retrieve the SoftwareProduct from the database based on the provided ID
            var softwareProduct = await _context.SoftwareProducts.FindAsync(license.SoftwareProductId);

            // Ensure that the softwareProduct exists
            if (softwareProduct == null)
            {
                // Handle the case where the specified SoftwareProduct does not exist
                throw new ArgumentException("SoftwareProduct not found");
            }

            // Associate the retrieved SoftwareProduct with the License
            // license.SoftwareProduct = softwareProduct;

            // Add the License to the DbContext
            _context.Licenses.Add(license);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return license;
        }
        public async Task<License> AssignManagersToLicenseAsync(int licenseId, int[] managerIds)
        {
            var license = await _context.Licenses
                .Include(l => l.LicenseManagers)
                .SingleOrDefaultAsync(l => l.Id == licenseId);

            if (license == null)
                return null;

            var managers = await _context.LicenseManagers
                .Where(lm => managerIds.Contains(lm.Id))
                .ToListAsync();

            foreach (var manager in managers)
            {
                license.LicenseManagers.Add(manager);
            }

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
