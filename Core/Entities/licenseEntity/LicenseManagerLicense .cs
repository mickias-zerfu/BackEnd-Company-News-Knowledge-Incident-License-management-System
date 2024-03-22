namespace Core.Entities.licenseEntity
{
    public class LicenseManagerLicense
    {
        public int LicenseId { get; set; }
        public License License { get; set; }

        public int LicenseManagerId { get; set; }
        public LicenseManager LicenseManager { get; set; }

    }
}