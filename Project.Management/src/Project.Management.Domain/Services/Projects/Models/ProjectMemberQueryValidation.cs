namespace Project.Management.Domain.Services.Projects.Models
{
    public class ProjectMemberQueryValidation
    {
        public Guid ProjectMemberId { get; set; }
        public bool UserExists { get; set; }
        public bool ProjectExists { get; set; }
        public bool RoleExists { get; set; }
    }
}
