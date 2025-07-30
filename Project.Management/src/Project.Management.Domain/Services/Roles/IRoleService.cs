using Project.Management.Domain.Entities;
using Project.Management.Domain.Services.Roles.Models;

namespace Project.Management.Domain.Services.Roles
{
    public interface IRoleService : IBaseRepositoryService<Role>
    {
        Task<Role> Create(RoleCreateRequest request);
        Task<Role> Update(RoleUpdateRequest request);
    }
}
