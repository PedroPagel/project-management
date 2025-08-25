using System.Diagnostics.CodeAnalysis;

namespace Project.Management.Api.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ProjectMemberDto
    {
        public Guid UserId { get; set; }
        public UserDto UserDto { get; set; }

        public Guid ProjectId { get; set; }
        public ProjectDto ProjectDto { get; set; }

        public Guid RoleId { get; set; }
        public RoleDto RoleDto { get; set; }

        public bool Active { get; set; }
    }
}
