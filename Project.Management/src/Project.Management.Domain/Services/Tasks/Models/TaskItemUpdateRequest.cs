using System.ComponentModel.DataAnnotations;

namespace Project.Management.Domain.Services.Tasks.Models
{
    public class TaskItemUpdateRequest
    {
        [Required]
        public Guid TaskId { get; set; }

        [Required]
        public Enums.TaskStatus Status { get; set; }

        public DateTime? DueDate { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
