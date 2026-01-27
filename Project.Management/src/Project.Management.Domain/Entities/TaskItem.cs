namespace Project.Management.Domain.Entities
{
    public class TaskItem : Entity
    {
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public Enums.TaskStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }

        public Guid? AssignedUserId { get; set; }
        public User AssignedUser { get; set; }
    }

}
