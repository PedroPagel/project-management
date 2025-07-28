using System.Diagnostics.CodeAnalysis;

namespace Project.Management.Api.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
