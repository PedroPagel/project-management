using Microsoft.EntityFrameworkCore;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Infrastructure.Data;

namespace Project.Management.Infrastructure.Repositories
{
    public class ProjectMemberRepository(ProjectManagementDbContext db) : Repository<ProjectMember>(db), IProjectMemberRepository
    {
        public async Task<bool> Delete(Guid userId, Guid id)
        {
            var projectMember = await DbSet.FirstOrDefaultAsync(pm => pm.Id == id && pm.UserId == userId);

            DbSet.Remove(projectMember);
            return await SaveChanges();
        }
    }
}
