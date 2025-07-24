using System.ComponentModel.DataAnnotations;

namespace Project.Management.Domain.Services.Projects.Models
{
    public class ProjectCreationRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
