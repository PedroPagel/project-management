using Project.Management.Domain.Entities;

namespace Project.Management.Domain.Repositories
{
    public interface IProjectMemberRepository : IRepository<ProjectMember>
    {
        public Task<bool> Delete(Guid userId, Guid id);
    }
}
