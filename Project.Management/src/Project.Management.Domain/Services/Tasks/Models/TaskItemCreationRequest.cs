using System.ComponentModel.DataAnnotations;

namespace Project.Management.Domain.Services.Tasks.Models
{
    public class TaskItemCreationRequest
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime? DueDate { get; set; }

        [Required]
        public Guid? AssignedUserId { get; set; }
    }
}
