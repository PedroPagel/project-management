namespace Project.Management.Domain.Entities
{
    public class ProjectMember : Entity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public bool Active { get; set; }
    }
}
