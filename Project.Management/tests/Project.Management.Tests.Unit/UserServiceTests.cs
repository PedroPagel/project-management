using Moq;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Users;

namespace Project.Management.Tests.Unit
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repoMock;
        private readonly Mock<INotificator> _notificatorMock;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _repoMock = new Mock<IUserRepository>();
            _notificatorMock = new Mock<INotificator>();

            _service = new UserService(_notificatorMock.Object, _repoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldAssignId_AndCreatedDate()
        {
            var user = new User { FullName = "Alice", Email = "alice@mail.com", PasswordHash = "123" };

            _repoMock.Setup(r => r.Create(It.IsAny<User>()))
             .ReturnsAsync((User u) =>
             {
                 u.Id = Guid.NewGuid();
                 u.CreatedDate = DateTime.UtcNow;
                 u.UserUpdated = "system";

                 return u;
             });

            var result = await _service.Create(user);

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.True(result.CreatedDate <= DateTime.UtcNow);
            Assert.Equal("system", result.UserUpdated);

            _repoMock.Verify(r => r.Create(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldCallRepository()
        {
            var users = new List<User> { new() { FullName = "Bob" } };
            _repoMock.Setup(r => r.GetAll()).ReturnsAsync(users);

            var result = await _service.GetAllUsers();

            Assert.Single(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteOnRepository()
        {
            var id = Guid.NewGuid();

            await _service.Delete(id);

            _repoMock.Verify(r => r.Delete(id), Times.Once);
        }
    }

}
