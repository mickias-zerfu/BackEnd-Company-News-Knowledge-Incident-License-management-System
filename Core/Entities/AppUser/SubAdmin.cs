using System.ComponentModel.DataAnnotations; 

namespace Core.Entities.AppUser
{
    public class SubAdmin
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
 
        public int[]? Access { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public int Status { get; set; }
        public DateTime Created_at { get; set; } // Change to DateTime
        public DateTime Updated_at { get; set; } // Change to DateTime
    }
}