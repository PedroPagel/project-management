using Microsoft.Extensions.Logging;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Extensions;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Users.Models;
using Project.Management.Domain.Services.Users.Validators;

namespace Project.Management.Domain.Services.Users
{
    public class UserService(INotificator notificator, ILogger<UserService> logger, IUserRepository userRepository) :
        BaseRepositoryService<User>(notificator, logger, userRepository), IUserService
    {
        public async Task<User> Create(UserCreationRequest request)
        {
            _logger.LogInformation("Creating user with email {Email}", request.Email);

            if (Validate(new UserCreationValidator(), request))
            {
                _logger.LogInformation("User creation validation passed for email {Email}", request.Email);
                var user = await _repository.FirstOrDefault(u => u.Email.Equals(request.Email));

                if (user != null)
                {
                    NotifyErrorConflict("A user with this email already exists");
                    return null;
                }

                return await _repository.Create(new User()
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PasswordHash = request.PasswordHash
                });
            }

            _logger.LogInformation("User creation failed due to validation errors");
            return null;
        }

        public async Task<User> Update(UserUpdateRequest request)
        {
            _logger.LogInformation("Updating user with ID {Id}", request.Id);

            if (Validate(new UserUpdateValidator(), request))
            {
                var user = await _repository.GetById(request.Id);

                if (user == null)
                {
                    NotifyErrorNotFound($"User not found with ID {request.Id} for proceeding to update");
                    return null;
                }

                user.UpdateIfDifferent(u => u.Email, request.Email);

                return await _repository.Update(user);
            }

            _logger.LogInformation("User update failed due to validation errors");
            return null;
        }
    }
}
