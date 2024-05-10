using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.licenseEntity;

namespace Core.Interfaces.licenses
{
    public interface ILicenseDashboardService
    {
        
        Task<LicenseDashboardCount> GetLicenseDashboardCounts();

    }
}