using Project.Management.Domain.Entities;

namespace Project.Management.Domain.Services.Tasks
{
    public interface ITaskItemService
    {
        Task<TaskItem> Create(TaskItem taskItem);
        Task<TaskItem> Update(TaskItem taskItem);
        Task<bool> Delete(Guid id);
        Task<TaskItem> GetById(Guid id);
        Task<IEnumerable<TaskItem>> GetAll();
    }

}
