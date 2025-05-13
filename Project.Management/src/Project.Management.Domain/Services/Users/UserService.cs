using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;

namespace Project.Management.Domain.Services.Users
{
    public class UserService(INotificator notificator, IUserRepository userRepository) : BaseService(notificator), IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<User> Create(User user)
        {
            return await _userRepository.Create(user);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _userRepository.Delete(id) > 0;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<User> Update(User user)
        {
            return await _userRepository.Update(user);
        }
    }
}
