using Microsoft.Extensions.Logging;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Extensions;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Roles.Models;
using Project.Management.Domain.Services.Roles.Validators;

namespace Project.Management.Domain.Services.Roles
{
    public class RoleService(INotificator notificator, IRoleRepository roleRepository, ILogger<RoleService> logger)
     : BaseRepositoryService<Role>(notificator, logger, roleRepository), IRoleService
    {
        public async Task<Role> Create(RoleCreateRequest request)
        {
            _logger.LogInformation("Creating role with name {Name}", request.Name);

            if (Validate(new RoleCreationValidator(), request))
            {
                _logger.LogInformation("Role creation validation passed for name {Name}", request.Name);
                var role = await _repository.FirstOrDefault(u => u.Name.Equals(request.Name));

                if (role != null)
                {
                    NotifyErrorConflict("A role with this name already exists");
                    return null;
                }

                return  await _repository.Create(new Role()
                {
                    Name = request.Name
                });
            }

            _logger.LogInformation("Role creation failed due to validation errors");
            return null;
        }

        public async Task<Role> Update(RoleUpdateRequest request)
        {
            _logger.LogInformation("Updating role with ID {Id}", request.Id);

            if (request.Id == Guid.Empty)
            {
                NotifyErrorBadRequest($"Invalid user Id provided");
                return null;
            }

            var role = await _repository.GetById(request.Id);

            if (role is null)
            {
                NotifyErrorNotFound($"Role not found with ID {request.Id} for proceeding to update");
                return null;
            }

            if (Validate(new RoleUpdateValidator(role), request))
            {
                _logger.LogInformation("Role update validation passed for role {Name}", request.Name);

                role.UpdateIfDifferent(u => u.Name, request.Name);

                return await _repository.Update(role);
            }

            _logger.LogInformation("Role update failed due to validation errors");
            return null;
        }
    }

}
