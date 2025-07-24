using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Infrastructure.Data;

namespace Project.Management.Infrastructure.Repositories
{
    public abstract class Repository<TEntity>(ProjectManagementDbContext dbContext) : IRepository<TEntity> where TEntity : Entity, new()
    {
        public readonly DbSet<TEntity> DbSet = dbContext.Set<TEntity>();
        private readonly ProjectManagementDbContext DbContext = dbContext;

        public virtual async Task<TEntity> Create(TEntity entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedDate = DateTime.UtcNow;

            await DbContext.AddAsync(entity);
            return entity;
        }

        public virtual async Task<int> Delete(Guid id)
        {
            var entity = await DbSet.FirstOrDefaultAsync(e => e.Id == id);

            if (entity is null) 
                return 0;

            DbContext.Remove(entity);
            return await DbContext.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;

            DbContext.Update(entity);
            await DbContext.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> SaveChanges()
        {
            return await DbContext.SaveChangesAsync() > 0;
        }
    }
}
