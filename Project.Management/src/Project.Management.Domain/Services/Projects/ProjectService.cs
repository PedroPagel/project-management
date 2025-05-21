using Microsoft.Extensions.Logging;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;

namespace Project.Management.Domain.Services.Projects
{
    public class ProjectService(INotificator notificator, IProjectRepository projectRepository, ILogger<ProjectService> logger)
    : BaseService(notificator, logger), IProjectService
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<Entities.Project> Create(Entities.Project project) => await _projectRepository.Create(project);

        public async Task<Entities.Project> Update(Entities.Project project) => await _projectRepository.Update(project);

        public async Task<bool> Delete(Guid id) => await _projectRepository.Delete(id) > 0;

        public async Task<Entities.Project> GetById(Guid id) => await _projectRepository.GetById(id);

        public async Task<IEnumerable<Entities.Project>> GetAll() => await _projectRepository.GetAll();
    }

}
