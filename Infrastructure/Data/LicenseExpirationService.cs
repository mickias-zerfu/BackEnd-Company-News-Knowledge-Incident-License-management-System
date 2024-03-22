using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Entities;
using Core.Interfaces.licenses;
using System.Net.Mail;
using System.Net;
using Core.Entities.licenseEntity;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class LicenseExpirationService : ILicenseExpirationService
    {
        private readonly StoreContext _context;
        private readonly ILicenseRepository _licenseRepository;
        private readonly ISoftwareProductRepository _softwareProductRepository;
        private readonly IEmailNotificationService _emailNotificationService;

        public LicenseExpirationService(ILicenseRepository licenseRepository,
            ISoftwareProductRepository softwareProductRepository,
            IEmailNotificationService emailNotificationService, StoreContext context)
        {
            _context = context;
            _licenseRepository = licenseRepository ?? throw new ArgumentNullException(nameof(licenseRepository));
            _softwareProductRepository = softwareProductRepository ?? throw new ArgumentNullException(nameof(softwareProductRepository));
            _emailNotificationService = emailNotificationService ?? throw new ArgumentNullException(nameof(emailNotificationService));
        }
        public async Task CheckLicenseExpirationAsync()
        {
            // Ensure that dependencies are not null
            if (_licenseRepository == null || _softwareProductRepository == null || _emailNotificationService == null)
            {
                throw new InvalidOperationException("Dependencies are not properly initialized.");
            }

            var licenses = await _licenseRepository.GetAllLicensesAsync();

            foreach (var license in licenses)
            {
                if (license == null || license.ExpirationDate == null)
                { 
                    continue;
                }

                var daysUntilExpiration = (license.ExpirationDate - DateTime.Today).Days;
                var softwareProduct = license.SoftwareProduct;
                var managers = license.AssignedManagers;
                //await _softwareProductRepository.GetSoftwareProductByIdAsync(license.SoftwareProductId);

                // Ensure that softwareProduct is not null before proceeding
                if (softwareProduct == null)
                {
                    // Log or handle the case where softwareProduct is null
                    continue;
                } 
                if (_emailNotificationService != null)
                { 
                    await _emailNotificationService.SendLicenseExpirationEmailAsync( softwareProduct,managers,license.ExpirationDate, daysUntilExpiration);
                }
                else
                {
                    // Log or handle the case where emailNotificationService is null
                }
            }
        }
 
     }
}
