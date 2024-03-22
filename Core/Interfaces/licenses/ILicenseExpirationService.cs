using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.licenseEntity;

namespace Core.Interfaces.licenses
{
    public interface ILicenseExpirationService
    {
        Task CheckLicenseExpirationAsync();
    }
    public interface IEmailNotificationService
    {
        Task SendLicenseExpirationEmailAsync(SoftwareProduct softwareProduct, ICollection<LicenseManager> licenseManager, DateTime expirationDate, int daysUntilExpiration);
    }

}