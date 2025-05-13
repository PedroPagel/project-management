namespace Project.Management.Api.Dtos
{
    public class TaskItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid AssignedUserId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }

}
