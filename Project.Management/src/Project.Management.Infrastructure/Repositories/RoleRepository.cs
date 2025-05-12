using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Infrastructure.Data;

namespace Project.Management.Infrastructure.Repositories
{
    public class RoleRepository(ProjectManagementDbContext db) : Repository<Role>(db), IRoleRepository
    {
    }
}
