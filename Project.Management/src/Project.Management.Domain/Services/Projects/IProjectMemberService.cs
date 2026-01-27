using Project.Management.Domain.Entities;
using Project.Management.Domain.Services.Projects.Models;

namespace Project.Management.Domain.Services.Projects
{
    public interface IProjectMemberService : IBaseRepositoryService<ProjectMember>
    {
        Task<ProjectMember> Create(ProjectMemberCreationRequest request);
        Task<ProjectMember> Update(ProjectMemberUpdateRequest request);
        Task<IEnumerable<ProjectMember>> GetProjectMember(ProjectMemberRequest projectMemberRequest);
    }
}
