using Project.Management.Domain.Entities;
using System.Linq.Expressions;

namespace Project.Management.Domain.Services.Projects
{
    public interface IProjectMemberService
    {
        Task<ProjectMember> Create(ProjectMember member);
        Task<ProjectMember> Update(ProjectMember member);
        Task<bool> Delete(Guid userId, Guid projectId);
        Task<ProjectMember> GetProject(Expression<Func<ProjectMember, bool>> predicate);
        Task<IEnumerable<ProjectMember>> GetProjects(Expression<Func<ProjectMember, bool>> predicate);
        Task<IEnumerable<ProjectMember>> GetAll();

        Task<ProjectMember> GetById(Guid userId, Guid projectId);
    }

}
