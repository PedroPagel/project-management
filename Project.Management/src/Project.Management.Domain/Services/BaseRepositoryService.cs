using Microsoft.Extensions.Logging;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;

namespace Project.Management.Domain.Services
{
    public abstract class BaseRepositoryService<TEntity>(INotificator notificator, ILogger<BaseService> logger, IRepository<TEntity> repository)
        : BaseService(notificator, logger) where TEntity : Entity
    {
        protected readonly IRepository<TEntity> _repository = repository;

        public virtual async Task<bool> Delete(Guid id)
        {
            _logger.LogInformation("Deleting {Entity} with ID {Id}", typeof(TEntity).Name, id);

            if (id == Guid.Empty)
            {
                NotifyErrorBadRequest($"Invalid {typeof(TEntity).Name} Id provided");
                return false;
            }

            var entity = await _repository.Delete(id);

            if (entity is 0)
            {
                _logger.LogInformation("{Entity} not deleted, with ID {Id}, not found.", typeof(TEntity).Name, id);
                return false;
            }

            _logger.LogInformation("{Entity} with ID {Id}, removed", typeof(TEntity).Name, id);
            return true;
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            _logger.LogInformation("Fetching {Entity} with ID {Id}", typeof(TEntity).Name, id);

            if (id == Guid.Empty)
            {
                NotifyErrorBadRequest($"Invalid {typeof(TEntity).Name} Id provided");
                return null;
            }

            var entity = await _repository.GetById(id);

            if (entity == null)
            {
                NotifyErrorNotFound($"{typeof(TEntity).Name} not found");
                return null;
            }

            _logger.LogInformation("{Entity} found with ID {Id}", typeof(TEntity).Name, id);
            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            _logger.LogInformation("Fetching all data from {Entity}", typeof(TEntity).Name);
            return await _repository.GetAll();
        }
    }
}
