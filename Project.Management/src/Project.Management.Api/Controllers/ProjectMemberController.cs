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
    [Route("api/project-member")]
    [ExcludeFromCodeCoverage]
    public class ProjectMemberController(IProjectMemberService service, IMapper mapper, INotificator notificator) : BaseController(notificator)
    {
        private readonly IProjectMemberService _service = service;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Get all project members in the system.
        /// </summary>
        /// <returns>Project members.</returns>
        [HttpGet("all-projects")]
        public async Task<ActionResult<IEnumerable<ProjectMemberDto>>> GetAll()
        {
            var projects = _mapper.Map<IEnumerable<ProjectMemberDto>>(await _service.GetAll());

            return await CustomResponse(projects);
        }

        /// <summary>
        /// Get project members for a project.
        /// </summary>
        /// <param name="request">Project member query.</param>
        /// <returns>Project members.</returns>
        [HttpGet("projects-members")]
        public async Task<ActionResult<IEnumerable<ProjectMemberDto>>> GetProjectMembers([FromQuery] ProjectMemberRequest request)
        {
            var projects = _mapper.Map<IEnumerable<ProjectMemberDto>>(await _service.GetProjectMember(request));

            return await CustomResponse(projects);
        }

        /// <summary>
        /// Add a project member.
        /// </summary>
        /// <param name="request">Project member details.</param>
        /// <returns>New project member.</returns>
        [HttpPost("create")]
        public async Task<ActionResult<ProjectMemberDto>> Create(ProjectMemberCreationRequest request)
        {
            var created = _mapper.Map<ProjectMemberDto>(await _service.Create(request));

            return await CustomResponse(created);
        }

        /// <summary>
        /// Update a project member.
        /// </summary>
        /// <param name="request">Project member details.</param>
        /// <returns>Updated project member.</returns>
        [HttpPost("update")]
        public async Task<ActionResult<ProjectMemberDto>> Update(ProjectMemberUpdateRequest request)
        {
            var updated = _mapper.Map<ProjectMemberDto>(await _service.Update(request));

            return await CustomResponse(updated);
        }

        /// <summary>
        /// Delete a project member by id.
        /// </summary>
        /// <param name="id">Project member id.</param>
        /// <returns>True if deleted.</returns>
        [HttpDelete("delete-by-id/{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            var deleted = await _service.Delete(id);

            return await CustomResponse(deleted);
        }
    }
}
