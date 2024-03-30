using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.licenseEntity
{
    public class LicenseDto
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
        public int SoftwareProductId { get; set; }
        public SoftwareProduct SoftwareProduct { get; set; }
        public ICollection<LicenseManager> AssignedManagers { get; set; }
    }
}