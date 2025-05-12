using Microsoft.EntityFrameworkCore;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Infrastructure.Data;

namespace Project.Management.Infrastructure.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        public readonly DbSet<TEntity> DbSet;
        public ProjectManagementDbContext Db;

        protected Repository(ProjectManagementDbContext db)
        {
            DbSet = db.Set<TEntity>();
            Db = db;
        }

        public void Dispose() => Db?.Dispose();

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await DbSet.AsNoTracking().ToListAsync();
        }
    }
}
