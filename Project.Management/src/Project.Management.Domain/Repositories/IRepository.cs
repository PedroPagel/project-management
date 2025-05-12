using Project.Management.Domain.Entities;

namespace Project.Management.Domain.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task<IEnumerable<TEntity>> GetAll();
    }
}
