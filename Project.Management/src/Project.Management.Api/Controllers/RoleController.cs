using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Management.Api.Dtos;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Services.Roles;

namespace Project.Management.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController(IRoleService service, IMapper mapper) : ControllerBase
    {
        private readonly IRoleService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll()
        {
            var roles = await _service.GetAll();
            return Ok(_mapper.Map<IEnumerable<RoleDto>>(roles));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetById(Guid id)
        {
            var role = await _service.GetById(id);
            return role is null ? NotFound() : Ok(_mapper.Map<RoleDto>(role));
        }

        [HttpPost]
        public async Task<ActionResult<RoleDto>> Create(RoleDto dto)
        {
            var created = await _service.Create(_mapper.Map<Role>(dto));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<RoleDto>(created));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RoleDto>> Update(Guid id, RoleDto dto)
        {
            if (id != dto.Id) 
                return BadRequest();

            var updated = await _service.Update(_mapper.Map<Role>(dto));
            return Ok(_mapper.Map<RoleDto>(updated));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _service.Delete(id) ? NoContent() : NotFound();
        }
    }

}
