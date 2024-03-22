using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Interfaces.licenses;

namespace Infrastructure.Data
{

    // Implementation of license expiration tracking service
    public class LicenseExpirationService : ILicenseExpirationService
    {
        private readonly ILicenseRepository _licenseRepository;
        private readonly ISoftwareProductRepository _softwareProductRepository;
        private readonly IEmailNotificationService _emailNotificationService;

        public LicenseExpirationService(ILicenseRepository licenseRepository, ISoftwareProductRepository softwareProductRepository, IEmailNotificationService emailNotificationService)
        {
            _licenseRepository = licenseRepository;
            _emailNotificationService = emailNotificationService;
        }


        public async Task CheckLicenseExpirationAsync()
        {
            // Retrieve all licenses
            var licenses = await _licenseRepository.GetAllLicensesAsync();
            var softwareProductIds = licenses.Select(l => l.SoftwareProductId).Distinct().ToList();

            // Check expiration date for each license
            foreach (var license in licenses)
            {
                var daysUntilExpiration = (license.ExpirationDate - DateTime.Today).Days; 
                var softwareProduct = await _softwareProductRepository.GetSoftwareProductByIdAsync(license.SoftwareProductId);
                // Console.WriteLine($"License for {softwareProduct.Name} expires in {daysUntilExpiration} days");

                // Send notifications based on time frames
                if (daysUntilExpiration == 90)
                {
                    await _emailNotificationService.SendLicenseExpirationEmailAsync(softwareProduct, license.ExpirationDate, "Three months remaining before expiration");
                }
                else if (daysUntilExpiration == 30)
                {
                    await _emailNotificationService.SendLicenseExpirationEmailAsync(softwareProduct, license.ExpirationDate, "One month remaining before expiration");
                }
                else if (daysUntilExpiration == 14)
                {
                    // Send daily reminders for two weeks remaining before expiration
                    for (int i = 0; i < 14; i++)
                    {
                        await _emailNotificationService.SendLicenseExpirationEmailAsync(softwareProduct, license.ExpirationDate, "Two weeks remaining before expiration");
                    }
                }
                else if (daysUntilExpiration == 7)
                {
                    await _emailNotificationService.SendLicenseExpirationEmailAsync(softwareProduct, license.ExpirationDate, "One week remaining before expiration");
                }
                else if (daysUntilExpiration == 3)
                {
                    await _emailNotificationService.SendLicenseExpirationEmailAsync(softwareProduct, license.ExpirationDate, "Three days remaining before expiration");
                }
            }
        }

    }

}