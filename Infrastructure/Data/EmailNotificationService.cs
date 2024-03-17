using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.licenseEntity;
using Core.Interfaces;
using Core.Interfaces.licenses;

namespace Infrastructure.Data
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly ILicenseManagerRepository _licenseManagerRepository;

        public EmailNotificationService(ILicenseManagerRepository licenseManagerRepository)
        {
            _licenseManagerRepository = licenseManagerRepository;
        }
        public async Task SendLicenseExpirationEmailAsync(SoftwareProduct softwareProduct, DateTime expirationDate, string message)
        {
            // Retrieve all license managers
            var licenseManagers = await _licenseManagerRepository.GetAllLicenseManagersAsync();

            // Send email notification to each license manager
            foreach (var licenseManager in licenseManagers)
            {
                // Assuming license manager's email is stored in the Email property
                var emailAddress = licenseManager.Email;

                // Send email notification
                // Example:
                // string productName = softwareProduct.Name;
                // await emailSender.SendEmailAsync(emailAddress, "License Expiration Notification", $"Your license for {productName} has expired on {expirationDate.ToShortDateString()}");
            }
        }



    }

}