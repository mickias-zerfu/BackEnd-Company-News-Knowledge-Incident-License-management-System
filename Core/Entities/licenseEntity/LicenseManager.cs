
namespace Core.Entities.licenseEntity
{
    public class LicenseManager
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePictureUrl { get; set; }

        // EF Relation
        public int LicenseId { get; set; }
        public License License { get; set; }
    }

}