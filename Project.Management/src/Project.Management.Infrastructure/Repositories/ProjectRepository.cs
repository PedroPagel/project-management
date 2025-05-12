using Project.Management.Domain.Repositories;
using Project.Management.Infrastructure.Data;

namespace Project.Management.Infrastructure.Repositories
{
    public class ProjectRepository(ProjectManagementDbContext db) : Repository<Domain.Entities.Project>(db), IProjectRepository
    {
    }
}
