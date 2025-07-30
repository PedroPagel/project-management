using System.Diagnostics.CodeAnalysis;

namespace Project.Management.Api.Dtos
{
    [ExcludeFromCodeCoverage]
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
