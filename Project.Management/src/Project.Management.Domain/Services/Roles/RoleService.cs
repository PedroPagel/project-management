using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;

namespace Project.Management.Domain.Services.Roles
{
    public class RoleService(INotificator notificator, IRoleRepository roleRepository)
     : BaseService(notificator), IRoleService
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<Role> Create(Role role) => await _roleRepository.Create(role);

        public async Task<Role> Update(Role role) => await _roleRepository.Update(role);

        public async Task<bool> Delete(Guid id) => await _roleRepository.Delete(id) > 0;

        public async Task<Role> GetById(Guid id) => await _roleRepository.GetById(id);

        public async Task<IEnumerable<Role>> GetAll() => await _roleRepository.GetAll();
    }

}
