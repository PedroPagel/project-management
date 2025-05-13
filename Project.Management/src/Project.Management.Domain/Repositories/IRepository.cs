using System.Linq.Expressions;
using Project.Management.Domain.Entities;

namespace Project.Management.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetById(Guid id);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> Create(TEntity entity);
        Task<int> Delete(Guid id);
    }
}
