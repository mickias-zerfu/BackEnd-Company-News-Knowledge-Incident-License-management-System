using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.licenseEntity
{
    public class SoftwareProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Vendor { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
