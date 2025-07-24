using System.ComponentModel.DataAnnotations;

namespace Project.Management.Domain.Services.Users.Models
{
    public class UserUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }
    }
}
