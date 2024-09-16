
using Core.Interfaces;
using Core.Entities;
using Core.Interfaces.licenses;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Licenses
{
    public class LicenseExpirationService : ILicenseExpirationService
    {
        private readonly StoreContext _context;
        private readonly ILicenseRepository _licenseRepository;
        private readonly ISoftwareProductRepository _softwareProductRepository;
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly ILogger<LicenseExpirationService> _logger;

        public LicenseExpirationService(ILicenseRepository licenseRepository,
            ISoftwareProductRepository softwareProductRepository,
            IEmailNotificationService emailNotificationService, StoreContext context,
            ILogger<LicenseExpirationService> logger)
        {
            _context = context;
            _licenseRepository = licenseRepository ?? throw new ArgumentNullException(nameof(licenseRepository));
            _softwareProductRepository = softwareProductRepository ?? throw new ArgumentNullException(nameof(softwareProductRepository));
            _emailNotificationService = emailNotificationService ?? throw new ArgumentNullException(nameof(emailNotificationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task CheckLicenseExpirationAsync()
        {
            _logger.LogInformation("CheckLicenseExpirationAsync started.");

            if (_licenseRepository == null || _softwareProductRepository == null || _emailNotificationService == null)
            {
                _logger.LogError("Dependencies are not properly initialized.");
                throw new InvalidOperationException("Dependencies are not properly initialized.");
            }

            var licenses = await _licenseRepository.GetAllLicensesAsync();

            var filteredLicenses = licenses.Where(license => license.AssignedManagers.Count >= 1).ToList();

            _logger.LogInformation("Retrieved {Count} licenses.", licenses.Count);
            _logger.LogInformation("Retrieved {Count} filtered licenses.", filteredLicenses.Count);

            foreach (var license in filteredLicenses)
            {
                if (license == null || license.ExpirationDate == null)
                {
                    continue;
                }

                var daysUntilExpiration = (license.ExpirationDate - DateTime.Today).Days;
                var softwareProduct = license.SoftwareProduct;
                var managers = license.AssignedManagers;

                if (softwareProduct == null)
                {
                    continue;
                }

                try
                {
                    if (_emailNotificationService != null && (daysUntilExpiration == 120 ||daysUntilExpiration == 90 || daysUntilExpiration == 60 || daysUntilExpiration == 30 || daysUntilExpiration == 15 || daysUntilExpiration == 7 || daysUntilExpiration < 7))
                    {
                        await _emailNotificationService.SendLicenseExpirationEmailAsync(softwareProduct, managers, license.ExpirationDate, daysUntilExpiration);
                        _logger.LogInformation("Email sent for license {LicenseId}.", license.Id);
                    }
                    else
                    {
                        _logger.LogError("Email notification service is not initialized.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while sending expiration email for license {LicenseId}.", license.Id);
                }
            }

        }
        public async Task CheckLicenseExpirationAsyncById(int licenseId)
        {
            _logger.LogInformation("CheckLicenseExpirationAsync started for LicenseId: {LicenseId}.", licenseId);

            var license = await _licenseRepository.GetLicenseByIdAsync(licenseId);

            if (license == null)
            {
                _logger.LogWarning("License with Id {LicenseId} not found.", licenseId);
                return; // Exit method if license not found
            }

            if (license.ExpirationDate == null)
            {
                _logger.LogWarning("License with Id {LicenseId} has no expiration date set.", licenseId);
                return; // Exit method if expiration date is not set
            }

            if (license.AssignedManagers == null || license.AssignedManagers.Count < 1)
            {
                _logger.LogWarning("License with Id {LicenseId} has no managers assigned.", licenseId);
                return; // Exit method if no managers are assigned
            }

            var daysUntilExpiration = (license.ExpirationDate - DateTime.Today).Days;
            var softwareProduct = license.SoftwareProduct;
            var managers = license.AssignedManagers;

            if (softwareProduct == null)
            {
                _logger.LogWarning("Software product for LicenseId {LicenseId} is not defined.", licenseId);
                return; // Exit method if the software product is not defined
            }

            try
            {
                if (_emailNotificationService != null)
                {
                    await _emailNotificationService.SendLicenseExpirationEmailAsync(softwareProduct, managers, license.ExpirationDate, daysUntilExpiration);
                    _logger.LogInformation("Email sent successfully for LicenseId {LicenseId}.", license.Id);
                }
                else
                {
                    _logger.LogError("Email notification service is not initialized for LicenseId {LicenseId}.", license.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending expiration email for LicenseId {LicenseId}.", license.Id);
            }
        }

    }
}
