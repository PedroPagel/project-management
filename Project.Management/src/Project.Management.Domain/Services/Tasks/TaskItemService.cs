using Microsoft.Extensions.Logging;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Extensions;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Tasks.Models;
using Project.Management.Domain.Services.Tasks.Validations;

namespace Project.Management.Domain.Services.Tasks
{
    public class TaskItemService(INotificator notificator, ITaskItemRepository taskItemRepository, ILogger<TaskItemService> logger)
    : BaseRepositoryService<TaskItem>(notificator, logger, taskItemRepository), ITaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepository = taskItemRepository;

        public async Task<TaskItem> Create(TaskItemCreationRequest request)
        {
            _logger.LogInformation("Creating task with description {Description}", request.Description);

            if (Validate(new TaskItemCreationValidation(), request))
            {
                return await _repository.Create(new TaskItem()
                {
                    DueDate = request.DueDate,
                    AssignedUserId = request.AssignedUserId,
                    Title = request.Title,
                    ProjectId = request.ProjectId,
                    Description = request.Description,
                    Status = Enums.TaskStatus.New
                });
            }

            _logger.LogInformation("Task item creation failed due to validation errors");
            return null;
        }

        public async Task<TaskItem> Update(TaskItemUpdateRequest request)
        {
            _logger.LogInformation("Updating task with id {TaskId}", request.TaskId);
            var taskItem = await _repository.GetById(request.TaskId);

            if (taskItem is null)
            {
                NotifyErrorBadRequest($"Invalid task item Id provided");
                return null;
            }

            if (Validate(new TaskItemUpdateValidation(taskItem.Status), request))
            {
                taskItem.UpdateIfDifferent(ti => ti.Description, request.Description);
                taskItem.UpdateIfDifferent(ti => ti.Title, request.Title);
                taskItem.UpdateIfDifferent(ti => ti.DueDate, request.DueDate);
                taskItem.Status = request.Status;

                return await _repository.Update(taskItem);
            }

            _logger.LogInformation("Task item update failed due to validation errors");
            return null;
        }
    }

}
