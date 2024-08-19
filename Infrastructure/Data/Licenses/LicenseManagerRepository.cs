
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

        public async Task<LicenseManagerDto> GetLicenseManagerByIdAsync(int id)
        {
            var licenseManager = await _context.LicenseManagers
                .Include(lm => lm.LicenseManagerLicenses)
                    .ThenInclude(lml => lml.License) // Include License navigation property within LicenseManagerLicenses 
                .FirstOrDefaultAsync(lm => lm.Id == id);
            if (licenseManager == null)
            {
                return null; // or throw an exception if desired
            }
            return new LicenseManagerDto
            {
                Id = licenseManager.Id,
                FirstName = licenseManager.FirstName,
                LastName = licenseManager.LastName,
                Email = licenseManager.Email,
                PhoneNumber = licenseManager.PhoneNumber,
                Role = licenseManager.Role,
                IsActive = licenseManager.IsActive,
                RegistrationDate = licenseManager.RegistrationDate,
                ProfilePictureUrl = licenseManager.ProfilePictureUrl,
                AssignedLicenses = licenseManager.LicenseManagerLicenses
                    .Select(lml => new License
                    {
                        Id = lml.License.Id,
                        IssuedTo = lml.License.IssuedTo,
                        IssuedBy = lml.License.IssuedBy,
                        ExpirationDate = lml.License.ExpirationDate
                    }).ToList()
            };
        }
        public async Task<IReadOnlyList<LicenseManagerDto>> GetAllLicenseManagersAsync()
        {
            return await _context.LicenseManagers
                .Include(lm => lm.LicenseManagerLicenses)
                    .ThenInclude(lml => lml.License)
                .Select(lm => new LicenseManagerDto
                {
                    Id = lm.Id,
                    FirstName = lm.FirstName,
                    LastName = lm.LastName,
                    Email = lm.Email,
                    PhoneNumber = lm.PhoneNumber,
                    Role = lm.Role,
                    IsActive = lm.IsActive,
                    RegistrationDate = lm.RegistrationDate,
                    ProfilePictureUrl = lm.ProfilePictureUrl,
                    AssignedLicenses = lm.LicenseManagerLicenses.Select(lml => new License
                    {
                        Id = lml.License.Id,
                        IssuedTo = lml.License.IssuedTo,
                        IssuedBy = lml.License.IssuedBy,
                        ExpirationDate = lml.License.ExpirationDate
                    }).ToList()
                }).ToListAsync(); 
        }

        public async Task<LicenseManager> CreateLicenseManagerAsync(LicenseManager licenseManager)
        {
            _context.LicenseManagers.Add(licenseManager);
            await _context.SaveChangesAsync();
            return licenseManager;
        }
        public async Task<LicenseManagerDto> AssignLicensesAsyncToManager(int managerId, int[] licenseIds)
        {
            // Find the license manager
            var licenseManager = await _context.LicenseManagers
                .Include(lm => lm.LicenseManagerLicenses) // Include existing assignments
                .FirstOrDefaultAsync(lm => lm.Id == managerId);

            if (licenseManager == null)
            {
                throw new ArgumentException("License Manager not found.");
            }

            // Get the licenses to be assigned
            var licenses = await _context.Licenses
                .Where(l => licenseIds.Contains(l.Id))
                .ToListAsync();

            if (licenses == null || licenses.Count == 0)
            {
                throw new InvalidOperationException("No licenses were found.");
            }

            // Clear existing assignments
            licenseManager.LicenseManagerLicenses.Clear();

            // Assign new licenses to the manager
            foreach (var license in licenses)
            {
                licenseManager.LicenseManagerLicenses.Add(new LicenseManagerLicense
                {
                    LicenseManagerId = managerId,
                    LicenseId = license.Id
                });
            }

            await _context.SaveChangesAsync();
            var licenseManagerDto = new LicenseManagerDto
            {
                Id = licenseManager.Id,
                FirstName = licenseManager.FirstName,
                LastName = licenseManager.LastName,
                Email = licenseManager.Email,
                PhoneNumber = licenseManager.PhoneNumber,
                Role = licenseManager.Role,
                IsActive = licenseManager.IsActive,
                RegistrationDate = licenseManager.RegistrationDate,
                ProfilePictureUrl = licenseManager.ProfilePictureUrl,
                AssignedLicenses = licenseManager.LicenseManagerLicenses
                    .Select(lml => new License
                    {
                        Id = lml.License.Id,
                        IssuedTo = lml.License.IssuedTo,
                        IssuedBy = lml.License.IssuedBy,
                        ExpirationDate = lml.License.ExpirationDate
                    }).ToList()
            };
            return licenseManagerDto;
        } 
        public async Task<LicenseManager> UpdateLicenseManagerAsync(LicenseManager licenseManager)
        {
            var existingLicenseManager = await _context.LicenseManagers.FindAsync(licenseManager.Id);
            if (existingLicenseManager == null)
            {
                throw new ArgumentException($"License manager with ID {licenseManager.Id} not found.");
            }
            _context.Entry(existingLicenseManager).State = EntityState.Detached;
            _context.Attach(licenseManager);
            _context.Entry(licenseManager).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return licenseManager;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflicts, if necessary
                throw new Exception("Concurrency conflict occurred");
            }
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