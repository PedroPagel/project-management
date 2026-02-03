using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Management.Api.Dtos;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Projects;
using Project.Management.Domain.Services.Projects.Models;
using System.Diagnostics.CodeAnalysis;

namespace Project.Management.Api.Controllers
{
    [ApiController]
    [Route("api/project")]
    [ExcludeFromCodeCoverage]
    public class ProjectController(INotificator notificator, IProjectService service, IMapper mapper) : BaseController(notificator)
    {
        private readonly IProjectService _service = service;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Get all projects in the system
        /// </summary>
        /// <returns>Projects</returns>
        [HttpGet("all-projects")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
        {
            var projects = _mapper.Map<IEnumerable<ProjectDto>>(await _service.GetAll());

            return await CustomResponse(projects);
        }

        /// <summary>
        /// Get project by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Project</returns>
        [HttpGet("project-by-id/{id}")]
        public async Task<ActionResult<ProjectDto>> GetById(Guid id)
        {
            var project = _mapper.Map<ProjectDto>(await _service.GetById(id));

            return await CustomResponse(project);
        }

        /// <summary>
        /// Add a project to the system
        /// </summary>
        /// <param name="request">Project details</param>
        /// <returns>New project</returns>
        [HttpPost("create")]
        public async Task<ActionResult<ProjectDto>> Create(ProjectCreationRequest request)
        {
            var created = _mapper.Map<ProjectDto>(await _service.Create(request));

            return await CustomResponse(created);
        }

        /// <summary>
        /// Update an existing project
        /// </summary>
        /// <param name="request">Project details</param>
        /// <returns>Updated project</returns>
        [HttpPut("update")]
        public async Task<ActionResult<ProjectDto>> Update(ProjectUpdateRequest request)
        {
            var updated = _mapper.Map<ProjectDto>(await _service.Update(request));

            return await CustomResponse(updated);
        }

        /// <summary>
        /// Delete a project from the system
        /// </summary>
        /// <param name="id">Project id</param>
        /// <returns>True if deleted</returns>
        [HttpDelete("delete-by-id/{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            var deletedProject = await _service.Delete(id);

            return await CustomResponse(deletedProject);
        }
    }

}
