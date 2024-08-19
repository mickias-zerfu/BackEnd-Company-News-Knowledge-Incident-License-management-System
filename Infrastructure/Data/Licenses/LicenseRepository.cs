using Core.Entities.licenseEntity;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID", nameof(id));
            }

            var license = await _context.Licenses
                .AsNoTracking()
                .Include(l => l.LicenseManagerLicenses)
                .ThenInclude(lml => lml.LicenseManager)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (license == null)
            {
                throw new ArgumentException("License not found.");
            }

            var licenseDto = new LicenseDto
            {
                Id = license.Id,
                IssuedTo = license.IssuedTo,
                IssuedBy = license.IssuedBy,
                CreationDate = license.CreationDate,
                ActivationDate = license.ActivationDate,
                ExpirationDate = license.ExpirationDate,
                MaxUsers = license.MaxUsers,
                Activated = license.Activated,
                LicenseType = license.LicenseType,
                Notes = license.Notes,
                SoftwareProductId = license.SoftwareProductId,
                SoftwareProduct = await _context.SoftwareProducts.AsNoTracking()
                                  .FirstOrDefaultAsync(sp => sp.Id == license.SoftwareProductId),
                AssignedManagers = license.LicenseManagerLicenses.Select(lml => new LicenseManager
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
                .AsNoTracking()
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
                    SoftwareProduct = _context.SoftwareProducts.AsNoTracking()
                                    .FirstOrDefault(sp => sp.Id == l.SoftwareProductId),
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
            if (license == null)
            {
                throw new ArgumentNullException(nameof(license), "License cannot be null.");
            }

            var softwareProduct = await _context.SoftwareProducts.FindAsync(license.SoftwareProductId);
            if (softwareProduct == null)
            {
                throw new ArgumentException("SoftwareProduct not found");
            }

            await _context.Licenses.AddAsync(license);
            await _context.SaveChangesAsync();

            return license;
        }
 
        public async Task<LicenseDto> AssignManagersToLicenseAsync(int licenseId, int[] managerIds)
        {
            if (licenseId <= 0 || managerIds == null || !managerIds.Any())
            {
                throw new ArgumentException("Invalid parameters");
            }

            // Load the license including related entities
            var license = await _context.Licenses
                .Include(l => l.LicenseManagerLicenses)
                    .ThenInclude(lml => lml.LicenseManager)
                .FirstOrDefaultAsync(l => l.Id == licenseId);

            if (license == null)
            {
                throw new ArgumentException("License not found.");
            }

            // Clear existing assignments
            license.LicenseManagerLicenses.Clear();

            var managers = await _context.LicenseManagers
                .Where(lm => managerIds.Contains(lm.Id))
                .ToListAsync();

            if (!managers.Any())
            {
                throw new ArgumentException("No managers found.");
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

            // Map to DTO
            var licenseDto = new LicenseDto
            {
                Id = license.Id,
                IssuedTo = license.IssuedTo,
                IssuedBy = license.IssuedBy,
                CreationDate = license.CreationDate,
                ActivationDate = license.ActivationDate,
                ExpirationDate = license.ExpirationDate,
                MaxUsers = license.MaxUsers,
                Activated = license.Activated,
                LicenseType = license.LicenseType,
                Notes = license.Notes,
                SoftwareProductId = license.SoftwareProductId,
                AssignedManagers = license.LicenseManagerLicenses.Select(lml => new LicenseManager
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




        public async Task<License> UpdateLicenseAsync(License license)
        {
            if (license == null)
            {
                throw new ArgumentNullException(nameof(license), "License cannot be null.");
            }

            var existingLicense = await _context.Licenses.FindAsync(license.Id);
            if (existingLicense == null)
            {
                throw new ArgumentException($"License with ID {license.Id} not found.");
            }

            // Update existing license properties
            existingLicense.IssuedTo = license.IssuedTo ?? existingLicense.IssuedTo;
            existingLicense.IssuedBy = license.IssuedBy ?? existingLicense.IssuedBy;
            existingLicense.CreationDate = license.CreationDate != DateTime.MinValue ? license.CreationDate : existingLicense.CreationDate;
            existingLicense.ActivationDate = license.ActivationDate ?? existingLicense.ActivationDate;
            existingLicense.ExpirationDate = license.ExpirationDate != DateTime.MinValue ? license.ExpirationDate : existingLicense.ExpirationDate;
            existingLicense.MaxUsers = license.MaxUsers > 0 ? license.MaxUsers : existingLicense.MaxUsers;
            existingLicense.Activated = license.Activated;
            existingLicense.LicenseType = license.LicenseType;
            existingLicense.Notes = license.Notes ?? existingLicense.Notes;
            existingLicense.SoftwareProductId = license.SoftwareProductId > 0 ? license.SoftwareProductId : existingLicense.SoftwareProductId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("Concurrency error occurred while updating the license.", ex);
            }

            return existingLicense;
        }

        public async Task DeleteLicenseAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID", nameof(id));
            }

            var licenseToDelete = await _context.Licenses.FindAsync(id);
            if (licenseToDelete != null)
            {
                _context.Licenses.Remove(licenseToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
