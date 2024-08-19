using System.Text.Json.Serialization;

namespace Core.Entities.licenseEntity
{
    public class LicenseManagerLicense
    {
        public int LicenseId { get; set; }
        public License License { get; set; }

        public int LicenseManagerId { get; set; }
        [JsonIgnore] 
        public LicenseManager LicenseManager { get; set; }

    }
}