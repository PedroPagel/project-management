using Project.Management.Domain.Entities;
using Project.Management.Domain.Services.Tasks.Models;

namespace Project.Management.Domain.Services.Tasks
{
    public interface ITaskItemService
    {
        Task<TaskItem> Create(TaskItemCreationRequest request);
        Task<TaskItem> Update(TaskItemUpdateRequest request);
        Task<bool> Delete(Guid id);
        Task<TaskItem> GetById(Guid id);
        Task<IEnumerable<TaskItem>> GetAll();
    }

}
