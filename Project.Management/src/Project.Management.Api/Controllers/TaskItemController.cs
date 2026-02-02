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
    /// <summary>
    /// Task item endpoints.
    /// </summary>
    public class TaskItemController(ITaskItemService service, IMapper mapper, INotificator notificator) : BaseController(notificator)
    {
        private readonly ITaskItemService _service = service;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Get all tasks in the system.
        /// </summary>
        /// <returns>Tasks.</returns>
        [HttpGet("all-tasks")]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetAll()
        {
            var tasks = _mapper.Map<IEnumerable<TaskItemDto>>(await _service.GetAll());

            return await CustomResponse(tasks);
        }

        /// <summary>
        /// Get a task by id.
        /// </summary>
        /// <param name="id">Task id.</param>
        /// <returns>Task.</returns>
        [HttpGet("task-by-id/{id}")]
        public async Task<ActionResult<TaskItemDto>> GetById(Guid id)
        {
            var task = _mapper.Map<TaskItemDto>(await _service.GetById(id));

            return await CustomResponse(task);
        }

        /// <summary>
        /// Add a task to the system.
        /// </summary>
        /// <param name="request">Task details.</param>
        /// <returns>New task.</returns>
        [HttpPost("add")]
        public async Task<ActionResult<TaskItemDto>> Create(TaskItemCreationRequest request)
        {
            var created = _mapper.Map<TaskItemDto>(await _service.Create(request));

            return await CustomResponse(created);
        }

        /// <summary>
        /// Update a task in the system.
        /// </summary>
        /// <param name="request">Task details.</param>
        /// <returns>Updated task.</returns>
        [HttpPut("update")]
        public async Task<ActionResult<TaskItemDto>> Update(TaskItemUpdateRequest request)
        {
            var updated = _mapper.Map<TaskItemDto>(await _service.Update(request));

            return await CustomResponse(updated);
        }

        /// <summary>
        /// Delete a task from the system.
        /// </summary>
        /// <param name="id">Task id.</param>
        /// <returns>True if deleted.</returns>
        [HttpDelete("delete-by-id/{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            var deleted = await _service.Delete(id);

            return await CustomResponse(deleted);
        }
    }

}
