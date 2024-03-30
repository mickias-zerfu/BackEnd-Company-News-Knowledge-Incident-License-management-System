
using System.Net;
using System.Net.Mail;
using Core.Entities.licenseEntity;
using Core.Interfaces;
using Core.Interfaces.licenses;

namespace Infrastructure.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly ILicenseManagerRepository _licenseManagerRepository;

        public EmailNotificationService(ILicenseManagerRepository licenseManagerRepository)
        {
            _licenseManagerRepository = licenseManagerRepository ?? throw new ArgumentNullException(nameof(licenseManagerRepository));
        }

        public async Task SendLicenseExpirationEmailAsync(SoftwareProduct softwareProduct, ICollection<LicenseManager> licenseManagers, DateTime expirationDate, int daysUntilExpiration)
        {
            // var managerEmails = licenseManagers.Select(m => m.Email).ToList();

            foreach (var manager in licenseManagers)
            {
                 
       
        string message = $"Dear {manager.FirstName} {manager.LastName},{Environment.NewLine}{Environment.NewLine}";
        message += $"This is to inform you that the license for {softwareProduct.Name} is {GetExpirationMessage(daysUntilExpiration)}.{Environment.NewLine}";
        message += $"The expiration date for the license is {expirationDate.ToShortDateString()}.{Environment.NewLine}";
        message += $"Please take appropriate action to ensure continuous access to our software products.{Environment.NewLine}";
        message += $"Thank you for your attention to this matter.{Environment.NewLine}{Environment.NewLine}";
        message += "Best regards,";
        message += $"{Environment.NewLine}Zemen Software Team,{Environment.NewLine}{Environment.NewLine}";
        message += "Lead By Mikiyas Zerfu";
                await SendEmailAsync(manager, "License Expiration Notification", message);
            }
        }

        private async Task SendEmailAsync(LicenseManager manager, string subject, string body)
        {
            try
            {
                var fromAddress = new MailAddress("mickiaszerfu@gmail.com", "Zemen Software License Expiration Tracker");
                var toAddress = new MailAddress(manager.Email, "" + manager.FirstName + " " + manager.LastName);
                const string fromPassword = "tono kbub zbvu oxgj";
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true, 
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };

                using (var message1 = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                })
                {
                    smtpClient.Send(message1);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
        private string GetExpirationMessage(int daysUntilExpiration)
        {
            return daysUntilExpiration switch
            {
                90 => "Your license for this software product is expiring in three months. Please take necessary actions to renew or extend the license.",
                30 => "Your license for this software product will expire in one month. Kindly ensure timely renewal or extension to avoid interruption of services.",
                14 => "Your license for this software product will expire in two weeks. We recommend planning ahead for license renewal or extension.",
                7 => "Your license for this software product is expiring in one week. Please make arrangements for renewal or extension to maintain uninterrupted access.",
                3 => "Your license for this software product will expire in three days. Immediate action is advised to prevent any service disruption.",
                _ => $"Your license for this software product is expiring in {daysUntilExpiration} days. Please plan accordingly for renewal or extension."
            };
        }
    }
}
