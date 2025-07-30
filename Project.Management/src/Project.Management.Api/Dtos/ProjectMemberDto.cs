using System.Diagnostics.CodeAnalysis;

namespace Project.Management.Api.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ProjectMemberDto
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid RoleId { get; set; }
    }

}
