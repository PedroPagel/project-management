using Project.Management.Domain.Entities;
using Project.Management.Domain.Services.Projects.Models;

namespace Project.Management.Domain.Repositories
{
    public interface IProjectMemberRepository : IRepository<ProjectMember>
    {
        Task<bool> Delete(Guid userId, Guid id);
        Task<ProjectMemberQueryValidation> GetProjectMemberLink(ProjectMemberCreationRequest projectmemberCreationRequest);
    }
}
