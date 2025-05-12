namespace Project.Management.Domain.Entities
{
    public class User : Entity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public ICollection<ProjectMember> ProjectMemberships { get; set; }
    }

}
