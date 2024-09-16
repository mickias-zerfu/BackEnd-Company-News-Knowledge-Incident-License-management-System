
using System.Net;
using System.Net.Mail;
using Core.Entities.licenseEntity;
using Core.Interfaces;
using Core.Interfaces.licenses;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Licenses
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly ILicenseManagerRepository _licenseManagerRepository;
        private readonly ILogger<EmailNotificationService> _logger;

        public EmailNotificationService(ILicenseManagerRepository licenseManagerRepository, ILogger<EmailNotificationService> logger)
        {
            _licenseManagerRepository = licenseManagerRepository ?? throw new ArgumentNullException(nameof(licenseManagerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        public async Task SendLicenseExpirationEmailAsync(SoftwareProduct softwareProduct, ICollection<LicenseManager> licenseManagers, DateTime expirationDate, int daysUntilExpiration)
        {
            // var managerEmails = licenseManagers.Select(m => m.Email).ToList();

            foreach (var manager in licenseManagers)
            {

                 

                string message = $"Dear {manager.FirstName} {manager.LastName},<br/><br/>";
                message += $"This is to inform you about the license for <strong>{softwareProduct.Name}</strong>.<br/><br/>";

                message += "<table style='border-collapse: collapse; width: 60%;'>";
                message += "<tr style='background-color: #f2f2f2;'><th style='border: 1px solid #ddd; padding: 8px;'>Item</th><th style='border: 1px solid #ddd; padding: 8px;'>Details</th></tr>";
                message += $"<tr><td style='border: 1px solid #ddd; padding: 8px;'>License Product</td><td style='border: 1px solid #ddd; padding: 8px;'><b>{softwareProduct.Name}</b></td></tr>";
                message += $"<tr><td style='border: 1px solid #ddd; padding: 8px;'>Expiration Date</td><td style='border: 1px solid #ddd; padding: 8px;'><b>{expirationDate.ToShortDateString()}</b></td></tr>";
                message += $"<tr><td style='border: 1px solid #ddd; padding: 8px;'>Days Until Expiration</td><td style='border: 1px solid #ddd; padding: 8px;'><b>{daysUntilExpiration}</b></td></tr>";
                message += "</table><br/>";

                message += $"{GetExpirationMessage(daysUntilExpiration)}.<br/><br/>";
                message += "<strong>Note:</strong> <br/><br/>";
                message += $" &emsp; Please take appropriate action to ensure continuous access to our software products.<br/><br/>";
                message += $" &emsp; If you have any questions or need assistance, please do not hesitate to contact us.<br/><br/>";
                message += "Best regards,<br/><br/>";

                message += "Zemen Online Banking Team<br/><br/><br/><br/>";
                await SendEmailAsync(manager, "License Expiration Notification", message);
            }
        }

        private async Task SendEmailAsync(LicenseManager manager, string subject, string body)
        {
            try
            {
                var fromAddress = new MailAddress("License.Notification@zemenbank.com", "License Notification");
                var toAddress = new MailAddress(manager.Email, $"{manager.FirstName} {manager.LastName}");

                var fromPassword = "P@ssw0rd";
                //Environment.GetEnvironmentVariable("EMAIL_PASSWORD"); // Retrieve password from secure storage

                var smtpClient = new SmtpClient("smtp.zemenbank.com")
                {
                    Host = "smtp.zemenbank.com",
                    Port = 25,
                    EnableSsl = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 40000 // Timeout set to 40 seconds
                };
                using (var message1 = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                })
                {
                    _logger.LogInformation("Attempting to send email to {Email} with subject {Subject}.", toAddress.Address, subject);

                    await smtpClient.SendMailAsync(message1); // Use SendMailAsync for async operation

                    _logger.LogInformation("Email successfully sent to {Email}.", toAddress.Address);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Failed to send email: {ex.Message}"); // Replace with logging
            }
        }

        private string GetExpirationMessage(int daysUntilExpiration)
        {
            if (daysUntilExpiration > 120)
            {
                return "You have more than 120 days before your license expires. You can chill out for now, but remember to renew or extend your license in time.";
            }
            else if (daysUntilExpiration > 90 && daysUntilExpiration <= 120)
            {
                return "Your license for this software product is expiring within the next Four months. Please take necessary actions to renew or extend the license.";
            }
            else if (daysUntilExpiration > 60 && daysUntilExpiration <= 90)
            {
                return "Your license for this software product is expiring within the next Three months. Please take necessary actions to renew or extend the license.";
            }
            else if (daysUntilExpiration > 30 && daysUntilExpiration <= 60)
            {
                return "Your license for this software product is expiring within the next Two months. Please take necessary actions to renew or extend the license.";
            }
            else if (daysUntilExpiration > 15 && daysUntilExpiration <= 30)
            {
                return "Your license for this software product is expiring within the next months. Please take necessary actions to renew or extend the license.";
            }
            else if (daysUntilExpiration > 10 && daysUntilExpiration <= 15)
            {
                return "Your license for this software product is expiring within the next two Weeks. Please take necessary actions to renew or extend the license.";
            }
            else if (daysUntilExpiration == 7)
            {
                return "Your license for this software product is expiring in less than a week. Please make arrangements for renewal or extension to maintain uninterrupted access.";
            }
            else if (daysUntilExpiration < 7)
            {
                return $"Your license for this software product is expiring in {daysUntilExpiration} days. Please plan accordingly for renewal or extension."; 
            }  
            else
                    {
                        return string.Empty;
                    }
            }
        }
    }

