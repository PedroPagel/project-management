using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Management.Api.Dtos;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Services.Projects;

namespace Project.Management.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectMemberController(IProjectMemberService service, IMapper mapper) : ControllerBase
    {
        private readonly IProjectMemberService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectMemberDto>>> GetAll()
        {
            var members = await _service.GetAll();
            return Ok(_mapper.Map<IEnumerable<ProjectMemberDto>>(members));
        }

        [HttpPost]
        public async Task<ActionResult<ProjectMemberDto>> Create(ProjectMemberDto dto)
        {
            var created = await _service.Create(_mapper.Map<ProjectMember>(dto));
            return Created("", _mapper.Map<ProjectMemberDto>(created)); 
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] ProjectMemberDto dto)
        {
            var removed = await _service.Delete(dto.UserId, dto.ProjectId);
            return removed ? NoContent() : NotFound();
        }
    }

}
