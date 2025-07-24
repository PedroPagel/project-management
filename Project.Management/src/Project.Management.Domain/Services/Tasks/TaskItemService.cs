using Microsoft.Extensions.Logging;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;

namespace Project.Management.Domain.Services.Tasks
{
    public class TaskItemService(INotificator notificator, ITaskItemRepository taskItemRepository, ILogger<TaskItemService> logger)
    : BaseService(notificator, logger), ITaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepository = taskItemRepository;

        public async Task<TaskItem> Create(TaskItem taskItem) => await _taskItemRepository.Create(taskItem);

        public async Task<TaskItem> Update(TaskItem taskItem) => await _taskItemRepository.Update(taskItem);

        public async Task<bool> Delete(Guid id) => await _taskItemRepository.Delete(id) > 0;

        public async Task<TaskItem> GetById(Guid id) => await _taskItemRepository.GetById(id);

        public async Task<IEnumerable<TaskItem>> GetAll() => await _taskItemRepository.GetAll();
    }

}
