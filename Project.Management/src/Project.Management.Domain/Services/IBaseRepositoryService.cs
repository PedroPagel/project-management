using Project.Management.Domain.Entities;

namespace Project.Management.Domain.Services
{
    public interface IBaseRepositoryService<TEntity> where TEntity : Entity
    {
        Task<bool> Delete(Guid id);
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> GetAll();
    }
}
