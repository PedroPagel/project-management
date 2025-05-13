using Project.Management.Domain.Entities;

namespace Project.Management.Domain.Services.Projects
{
    public interface IProjectMemberService
    {
        Task<ProjectMember> Create(ProjectMember member);
        Task<ProjectMember> Update(ProjectMember member);
        Task<bool> Delete(Guid userId, Guid projectId);
        Task<ProjectMember> GetById(Guid userId, Guid projectId);
        Task<IEnumerable<ProjectMember>> GetAll();
    }

}
