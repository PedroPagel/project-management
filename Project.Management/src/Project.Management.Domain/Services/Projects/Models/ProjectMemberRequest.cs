namespace Project.Management.Domain.Services.Projects.Models
{
    public class ProjectMemberRequest
    {
        public Guid ProjectMemberId { get; set; }

        public Guid ProjectId { get; set; }

        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }
    }
}
