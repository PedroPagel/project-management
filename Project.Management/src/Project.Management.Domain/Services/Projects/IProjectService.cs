using Project.Management.Domain.Services.Projects.Models;

namespace Project.Management.Domain.Services.Projects
{
    public interface IProjectService : IBaseRepositoryService<Entities.Project>
    {
        Task<Entities.Project> Create(ProjectCreationRequest request);
        Task<Entities.Project> Update(ProjectUpdateRequest request);
    }
}
