using System.ComponentModel.DataAnnotations;

namespace Project.Management.Domain.Services.Projects.Models
{
    public class ProjectMemberUpdateRequest
    {
        [Required]
        public Guid ProjectMemberId { get; set; }

        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
