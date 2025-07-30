using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Management.Api.Dtos;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Services.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace Project.Management.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ExcludeFromCodeCoverage]
    public class TaskItemController(ITaskItemService service, IMapper mapper) : ControllerBase
    {
        private readonly ITaskItemService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetAll()
        {
            var tasks = await _service.GetAll();
            return Ok(_mapper.Map<IEnumerable<TaskItemDto>>(tasks));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemDto>> GetById(Guid id)
        {
            var task = await _service.GetById(id);
            return task is null ? NotFound() : Ok(_mapper.Map<TaskItemDto>(task));
        }

        [HttpPost]
        public async Task<ActionResult<TaskItemDto>> Create(TaskItemDto dto)
        {
            var created = await _service.Create(_mapper.Map<TaskItem>(dto));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<TaskItemDto>(created));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskItemDto>> Update(Guid id, TaskItemDto dto)
        {
            if (id != dto.Id) 
                return BadRequest();

            var updated = await _service.Update(_mapper.Map<TaskItem>(dto));
            return Ok(_mapper.Map<TaskItemDto>(updated));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _service.Delete(id) ? NoContent() : NotFound();
        }
    }

}
