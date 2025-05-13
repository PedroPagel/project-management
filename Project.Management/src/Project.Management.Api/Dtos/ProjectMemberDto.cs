namespace Project.Management.Api.Dtos
{
    public class ProjectMemberDto
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid RoleId { get; set; }
    }

}
