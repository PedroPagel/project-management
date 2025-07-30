using System.Diagnostics.CodeAnalysis;

namespace Project.Management.Api.Dtos
{
    [ExcludeFromCodeCoverage]
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
