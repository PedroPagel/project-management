using System.Diagnostics.CodeAnalysis;

namespace Project.Management.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public abstract class Entity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UserUpdated { get; set; }
    }
}
