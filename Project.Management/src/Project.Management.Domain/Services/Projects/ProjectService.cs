using Microsoft.Extensions.Logging;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Extensions;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Projects.Models;
using Project.Management.Domain.Services.Projects.Validators;

namespace Project.Management.Domain.Services.Projects
{
    public class ProjectService(INotificator notificator, IProjectRepository projectRepository, ILogger<ProjectService> logger)
    : BaseService(notificator, logger), IProjectService
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly ILogger<ProjectService> _logger = logger;

        private async Task<(Entities.Project, bool HasConflict)> GetOnGoingProject(string name)
        {
            _logger.LogInformation("Fetching ongoing project with name {Name}", name);
            var project = await _projectRepository.FirstOrDefault(p => p.Name.Equals(name) &&
                !(p.Status == Enums.ProjectStatus.Completed || p.Status == Enums.ProjectStatus.Cancelled));

            if (project != null)
            {
                NotifyErrorConflict("A project with this name already going on.");
                return (null, true);
            }

            return (project, false);
        }

        public async Task<Entities.Project> Create(ProjectCreationRequest request)
        {
            _logger.LogInformation("Creating project with name {Name}", request.Name);

            if (Validate(new ProjectCreationValidator(), request))
            {
                _logger.LogInformation("Project creation validation passed for project {Name}", request.Name);
                var (project, hasConflict) = await GetOnGoingProject(request.Name);

                if (hasConflict || project is null)
                {
                    return null;
                }

                return await _projectRepository.Create(new Entities.Project()
                {
                    Name = request.Name,
                    Description = request.Description,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate
                });
            }

            _logger.LogInformation("Project creation failed due to validation errors");
            return null;;
        }

        public async Task<Entities.Project> Update(ProjectUpdateRequest request)
        {
            _logger.LogInformation("Updating project with ID {Id}", request.Id);

            var projectById = await _projectRepository.GetById(request.Id);

            if (projectById == null)
            {
                NotifyErrorNotFound($"Project not found with ID {request.Id} for proceeding to update");
                return null;
            }

            var (project, hasConflict) = await GetOnGoingProject(request.Name);

            if (hasConflict || project is null)
            {
                return null;
            }

            if (Validate(new ProjectUpdateValidator(project), request))
            {
                _logger.LogInformation("Project update validation passed for project {Name}", request.Name);

                project.UpdateIfDifferent(u => u.Name, request.Name);
                project.UpdateIfDifferent(u => u.Description, request.Description);
                project.UpdateIfDifferent(u => u.StartDate, request.StartDate);
                project.UpdateIfDifferent(u => u.EndDate, request.EndDate);

                return await _projectRepository.Update(project);
            }

            _logger.LogInformation("User update failed due to validation errors");
            return null;
        }

        public async Task<bool> Delete(Guid id) => await _projectRepository.Delete(id) > 0;

        public async Task<Entities.Project> GetById(Guid id) => await _projectRepository.GetById(id);

        public async Task<IEnumerable<Entities.Project>> GetAll() => await _projectRepository.GetAll();
    }

}
