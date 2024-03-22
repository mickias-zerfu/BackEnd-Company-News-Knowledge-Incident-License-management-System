using Core.Entities.licenseEntity;
using Core.Interfaces; 
using Microsoft.EntityFrameworkCore; 

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
            var l = await _context.Licenses.Include(l => l.LicenseManagerLicenses)
            .ThenInclude(lml => lml.LicenseManager)
            .FirstOrDefaultAsync(l => l.Id == id);
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
                AssignedManagers = l.LicenseManagerLicenses.Select(lml => new LicenseManager
                {
                    Id = lml.LicenseManager.Id,
                    FirstName = lml.LicenseManager.FirstName,
                    LastName = lml.LicenseManager.LastName,
                    Email = lml.LicenseManager.Email,
                    PhoneNumber = lml.LicenseManager.PhoneNumber
                }).ToList()
            };
            return licenseDto;
        }
        public async Task<IReadOnlyList<LicenseDto>> GetAllLicensesAsync()
        {
            return await _context.Licenses
                .Include(l => l.LicenseManagerLicenses) 
                    .ThenInclude(lml => lml.LicenseManager)
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
                    SoftwareProduct = _context.SoftwareProducts.FirstOrDefault(sp => sp.Id == l.SoftwareProductId),
                    AssignedManagers = l.LicenseManagerLicenses.Select(lml => new LicenseManager
                    {
                        Id = lml.LicenseManager.Id,
                        FirstName = lml.LicenseManager.FirstName,
                        LastName = lml.LicenseManager.LastName,
                        Email = lml.LicenseManager.Email,
                        PhoneNumber = lml.LicenseManager.PhoneNumber
                    }).ToList()
                })
                .ToListAsync();
        }
        public async Task<License> CreateLicenseAsync(License license)
        {
            var softwareProduct = await _context.SoftwareProducts.FindAsync(license.SoftwareProductId);

            // Ensure that the softwareProduct exists
            if (softwareProduct == null)
            {
                throw new ArgumentException("SoftwareProduct not found");
            }
            _context.Licenses.Add(license);
            await _context.SaveChangesAsync();

            return license;
        }
        public async Task<License> AssignManagersToLicenseAsync(int licenseId, int[] managerIds)
        {
            var license = await _context.Licenses.FindAsync(licenseId);
            if (license == null)
            {
                throw new ArgumentException("License not found.");
            }

            var managers = await _context.LicenseManagers
                .Where(lm => managerIds.Contains(lm.Id))
                .ToListAsync();
            if (managers == null)
            {
                // Log or handle the case where no managers were found
                throw new InvalidOperationException("No managers were found.");
            }
            // Clear existing assignments
            if (license.LicenseManagerLicenses == null)
            {
                license.LicenseManagerLicenses = new List<LicenseManagerLicense>();
            }
            foreach (var manager in managers)
            {
                license.LicenseManagerLicenses.Add(new LicenseManagerLicense
                {
                    LicenseId = licenseId,
                    LicenseManagerId = manager.Id
                });
            }

            await _context.SaveChangesAsync();
            return license;
        }
        public async Task<License> UpdateLicenseAsync(License license)
        {
            
            var existingLicense = await _context.Licenses.FindAsync(license.Id);
            if (existingLicense == null)
            {
                throw new ArgumentException($"License with ID {license.Id} not found.");
            }
            _context.Entry(existingLicense).State = EntityState.Detached;
            _context.Attach(license); 
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
