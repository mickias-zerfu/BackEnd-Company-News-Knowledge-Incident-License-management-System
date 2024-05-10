using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.AppUser
{
    public class SubAdmin : IdentityUser
    { 
        public string DisplayName { get; set; }

        public int[]? Access { get; set; } 
        public int RoleId { get; set; }
        public int Status { get; set; }
        public DateTime Created_at { get; set; } // Change to DateTime
        public DateTime Updated_at { get; set; } // Change to DateTime
    }
}