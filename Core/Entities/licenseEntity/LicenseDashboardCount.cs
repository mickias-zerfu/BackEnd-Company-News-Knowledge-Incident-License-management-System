using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace  Core.Entities.licenseEntity
{
    public class LicenseDashboardCount
    {
        public int LicenseCount { get; set; }
        public int LicenseManagerCount { get; set; }
        public int SoftwareCount { get; set; }
        public int ExpiredCount { get; set; }        
    }
}