using System.ComponentModel.DataAnnotations;

namespace Project.Management.Domain.Services.Users.Models
{
    public class UserCreationRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
