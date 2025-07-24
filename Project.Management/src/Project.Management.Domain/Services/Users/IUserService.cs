using Project.Management.Domain.Entities;
using Project.Management.Domain.Services.Users.Models;

namespace Project.Management.Domain.Services.Users
{
    public interface IUserService
    {
        Task<User> GetById(Guid id);
        Task<IEnumerable<User>> GetAll();
        Task<User> Create(UserCreationRequest request);
        Task<User> Update(UserUpdateRequest request);
        Task<bool> Delete(Guid id);
    }

}
