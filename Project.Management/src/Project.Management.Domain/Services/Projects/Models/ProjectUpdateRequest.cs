﻿using System.ComponentModel.DataAnnotations;

namespace Project.Management.Domain.Services.Projects.Models
{
    public class ProjectUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
