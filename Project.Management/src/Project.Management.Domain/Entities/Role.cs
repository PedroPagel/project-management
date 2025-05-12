namespace Project.Management.Domain.Entities
{
    public class Role : Entity
    {
        public string Name { get; set; }

        public ICollection<ProjectMember> ProjectMembers { get; set; }
    }
}
