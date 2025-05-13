namespace Project.Management.Domain.Services.Projects
{
    public interface IProjectService
    {
        Task<Entities.Project> Create(Entities.Project project);
        Task<Entities.Project> Update(Entities.Project project);
        Task<bool> Delete(Guid id);
        Task<Entities.Project> GetById(Guid id);
        Task<IEnumerable<Entities.Project>> GetAll();
    }

}
