using Microsoft.Extensions.Logging;
using Moq;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Users;
using Project.Management.Domain.Services.Users.Models;

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

            _service = new UserService(_notificatorMock.Object, new Mock<ILogger<UserService>>().Object, _repoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldAssignId_AndCreatedDate()
        {
            var request = new UserCreationRequest { FullName = "Alice", Email = "alice@mail.com", PasswordHash = "123456" };

            _repoMock.Setup(r => r.Create(It.IsAny<User>()))
             .ReturnsAsync((User u) =>
             {
                 u.Id = Guid.NewGuid();
                 u.CreatedDate = DateTime.UtcNow;
                 u.UserUpdated = "system";

                 return u;
             });

            var result = await _service.Create(request);

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

            var result = await _service.GetAll();

            Assert.Single(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteOnRepository()
        {
            var id = Guid.NewGuid();

            _repoMock.Setup(r => r.GetById(id)).ReturnsAsync(new User
            {
                Id = id,
                FullName = "Bob",
                CreatedDate = DateTime.UtcNow,
                UserUpdated = "system"
            });

            await _service.Delete(id);

            _repoMock.Verify(r => r.Delete(id), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnNull_WhenValidationFails()
        {
            var request = new UserCreationRequest { FullName = "", Email = "invalid", PasswordHash = "123" };

            var result = await _service.Create(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.Create(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnNull_WhenEmailAlreadyExists()
        {
            var request = new UserCreationRequest { FullName = "Alice", Email = "alice@mail.com", PasswordHash = "123456" };

            _repoMock.Setup(r => r.FirstOrDefault(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>()))
                .ReturnsAsync(new User { Id = Guid.NewGuid(), Email = request.Email });

            var result = await _service.Create(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.Create(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenIdIsEmpty()
        {
            var request = new UserUpdateRequest { Id = Guid.Empty, Email = "test@example.com" };

            var result = await _service.Update(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.Update(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenValidationFails()
        {
            var request = new UserUpdateRequest { Id = Guid.NewGuid(), Email = "invalid" };

            var result = await _service.Update(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Never);
            _repoMock.Verify(r => r.Update(It.IsAny<User>()), Times.Never);
        }
    }

}
