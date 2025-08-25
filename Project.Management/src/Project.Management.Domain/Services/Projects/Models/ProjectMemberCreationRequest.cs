using System.ComponentModel.DataAnnotations;

namespace Project.Management.Domain.Services.Projects.Models
{
    public class ProjectMemberCreationRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public Guid RoleId { get; set; }
    }
}
