using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Incident : BaseEntity
    {
        public string IncidentTitle { get; set; }
        public string IncidentDescription { get; set; }
        public string[] StatusAction { get; set; }
        public string[] QuickReviews { get; set; }
        public string[] SolutionToIncident { get; set; }
        public string Remark { get; set; }
    }
}