using Project.Management.Domain.Entities;

namespace Project.Management.Domain.Services.Users
{
    public interface IUserService
    {
        Task<User> GetUserById(Guid id);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> Create(User user);
        Task<User> Update(User user);
        Task<bool> Delete(Guid id);
    }

}
