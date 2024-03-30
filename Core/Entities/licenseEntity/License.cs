
using System.Text.Json.Serialization;

namespace Core.Entities.licenseEntity
{
    public class License
    {
        public int Id { get; set; }
        public string IssuedTo { get; set; }
        public string IssuedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int MaxUsers { get; set; }
        public bool Activated { get; set; }
        public LicenseType LicenseType { get; set; }
        public string Notes { get; set; }

        // EF Relation
        public int SoftwareProductId { get; set; }
        public ICollection<LicenseManagerLicense>? LicenseManagerLicenses { get; set; }

    }

    public enum LicenseType
    {
        SingleUserSubscription,
        MultiUserSubscription,
        SingleUserLifeTimeAccess,
        MultiUserLifeTimeAccess
    }

}