using Project.Management.Domain.Enums;

namespace Project.Management.Domain.Entities
{
    public class Project : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ICollection<TaskItem> Tasks { get; set; }
        public ICollection<ProjectMember> Members { get; set; }
    }

}
