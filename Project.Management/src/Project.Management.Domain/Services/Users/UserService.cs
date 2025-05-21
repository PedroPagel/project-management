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
        BaseService(notificator, logger), IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ILogger<UserService> _logger = logger;

        public async Task<User> Create(UserCreationRequest request)
        {
            _logger.LogInformation("Creating user with email {Email}", request.Email);

            if (Validate(new UserCreationValidator(), request))
            {
                _logger.LogInformation("User creation validation passed for email {Email}", request.Email);
                var user = await _userRepository.FirstOrDefault(u => u.Email.Equals(request.Email));

                if (user != null)
                {
                    NotifyErrorConflict("A user with this email already exists");
                    return null;
                }

                return await _userRepository.Create(new User()
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PasswordHash = request.PasswordHash
                });
            }

            _logger.LogInformation("User creation failed due to validation errors");
            return null;
        }

        public async Task<bool> Delete(Guid id)
        {
            _logger.LogInformation("Deleting user with ID {Id}", id);
            var user = await _userRepository.GetById(id);

            if (user is null)
                return false;

            _logger.LogInformation("User found with ID {Id}, proceeding to delete", id);
            return await _userRepository.Delete(id) > 0;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            _logger.LogInformation("Fetching all users");
            return await _userRepository.GetAll();
        }

        public async Task<User> GetUserById(Guid id)
        {
            _logger.LogInformation("Fetching user with ID {Id}", id);

            if (id == Guid.Empty)
            {
                NotifyErrorBadRequest("Invalid user Id provided");
                return null;
            }

            var user = await _userRepository.GetById(id);

            if (user == null)
            {
                NotifyErrorNotFound("User not found");
                return null;
            }

            _logger.LogInformation("User found with ID {Id}", id);
            return user;
        }

        public async Task<User> Update(UserUpdateRequest request)
        {
            _logger.LogInformation("Updating user with ID {Id}", request.Id);

            if (Validate(new UserUpdateValidator(), request))
            {
                var user = await _userRepository.GetById(request.Id);

                if (user == null)
                {
                    NotifyErrorNotFound($"User found with ID {request.Id} for proceeding to update");
                    return null;
                }

                user.UpdateIfDifferent(u => u.Email, request.Email);

                return await _userRepository.Update(user);
            }

            _logger.LogInformation("User update failed due to validation errors");
            return null;
        }
    }
}
