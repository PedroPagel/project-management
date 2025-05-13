using Project.Management.Domain.Entities;

namespace Project.Management.Domain.Services.Roles
{
    public interface IRoleService
    {
        Task<Role> Create(Role role);
        Task<Role> Update(Role role);
        Task<bool> Delete(Guid id);
        Task<Role> GetById(Guid id);
        Task<IEnumerable<Role>> GetAll();
    }

}
