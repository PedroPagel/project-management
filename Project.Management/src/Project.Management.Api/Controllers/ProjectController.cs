using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Management.Api.Dtos;
using Project.Management.Domain.Services.Projects;

namespace Project.Management.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController(IProjectService service, IMapper mapper) : ControllerBase
    {
        private readonly IProjectService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
        {
            var projects = await _service.GetAll();
            return Ok(_mapper.Map<IEnumerable<ProjectDto>>(projects));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetById(Guid id)
        {
            var project = await _service.GetById(id);
            return project is null ? NotFound() : Ok(_mapper.Map<ProjectDto>(project));
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Create(ProjectDto dto)
        {
            var created = await _service.Create(_mapper.Map<Domain.Entities.Project>(dto));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<ProjectDto>(created));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectDto>> Update(Guid id, ProjectDto dto)
        {
            if (id != dto.Id) 
                return BadRequest();

            var updated = await _service.Update(_mapper.Map<Domain.Entities.Project>(dto));
            return Ok(_mapper.Map<ProjectDto>(updated));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _service.Delete(id) ? NoContent() : NotFound();
        }
    }

}
