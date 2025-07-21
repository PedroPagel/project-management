using Project.Management.Domain.Services.Projects.Models;

namespace Project.Management.Domain.Services.Projects
{
    public interface IProjectService
    {
        Task<Entities.Project> Create(ProjectCreationRequest request);
        Task<Entities.Project> Update(ProjectUpdateRequest request);
        Task<bool> Delete(Guid id);
        Task<Entities.Project> GetById(Guid id);
        Task<IEnumerable<Entities.Project>> GetAll();
    }

}
