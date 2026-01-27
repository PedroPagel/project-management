using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Management.Api.Dtos;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Tasks;
using Project.Management.Domain.Services.Tasks.Models;
using System.Diagnostics.CodeAnalysis;

namespace Project.Management.Api.Controllers
{
    [ApiController]
    [Route("api/task")]
    [ExcludeFromCodeCoverage]
    public class TaskItemController(ITaskItemService service, IMapper mapper, INotificator notificator) : BaseController(notificator)
    {
        private readonly ITaskItemService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet("all-tasks")]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetAll()
        {
            var tasks = _mapper.Map<IEnumerable<TaskItemDto>>(await _service.GetAll());

            return await CustomResponse(tasks);
        }

        [HttpGet("task-by-id/{id}")]
        public async Task<ActionResult<TaskItemDto>> GetById(Guid id)
        {
            var task = _mapper.Map<TaskItemDto>(await _service.GetById(id));

            return await CustomResponse(task);
        }

        [HttpPost("add")]
        public async Task<ActionResult<TaskItemDto>> Create(TaskItemCreationRequest request)
        {
            var created = _mapper.Map<TaskItemDto>(await _service.Create(request));

            return await CustomResponse(created);
        }

        [HttpPut("update")]
        public async Task<ActionResult<TaskItemDto>> Update(TaskItemUpdateRequest request)
        {
            var updated = _mapper.Map<TaskItem>(await _service.Update(request));

            return await CustomResponse(updated);
        }

        [HttpDelete("delete-by-id/{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            var deleted = await _service.Delete(id);

            return await CustomResponse(deleted);
        }
    }

}
