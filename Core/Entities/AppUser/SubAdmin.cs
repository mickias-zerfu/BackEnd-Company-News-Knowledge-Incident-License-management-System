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

        public List<string> Access { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
    }
}