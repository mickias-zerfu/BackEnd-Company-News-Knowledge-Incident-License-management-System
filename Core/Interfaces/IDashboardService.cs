using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IDashboardService
    {

        Task<DashboardCounts> GetDashboardCounts();
        Task<List<MonthlyUploads>> GetMonthlyUploads();
    }
}