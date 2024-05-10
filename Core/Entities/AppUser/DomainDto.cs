using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.AppUser
{
    public class DomainDto
     {
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public string? Token { get; set; }
        public int Status { get; set; }
        public string? Message { get; set; }
        public int? RoleId { get; set; }  
    }
}