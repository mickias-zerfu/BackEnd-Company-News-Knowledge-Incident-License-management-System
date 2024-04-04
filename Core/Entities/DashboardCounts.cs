using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DashboardCounts
    {
        public int NewsCount { get; set; }
        public int IncidentCount { get; set; }
        public int SharedResourceCount { get; set; }
        public int KnowledgeBaseCount { get; set; }
    }
    public class MonthlyUploads
    {
        public string Month { get; set; }
        public int NewsUploads { get; set; }
        public int IncidentUploads { get; set; }
        public int SharedResourceUploads { get; set; }
        public int KnowledgeBaseUploads { get; set; }
    }
}