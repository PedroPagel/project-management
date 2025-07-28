using Microsoft.Extensions.Logging;
using Moq;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Roles;
using Project.Management.Domain.Services.Roles.Models;

namespace Project.Management.Tests.Unit
{
    public class RoleServiceTests
    {
        private readonly Mock<IRoleRepository> _repoMock;
        private readonly Mock<INotificator> _notificatorMock;
        private readonly RoleService _service;

        public RoleServiceTests()
        {
            _repoMock = new Mock<IRoleRepository>();
            _notificatorMock = new Mock<INotificator>();
            _service = new RoleService(_notificatorMock.Object, _repoMock.Object, new Mock<ILogger<RoleService>>().Object);
        }

        [Fact]
        public async Task Create_ShouldCallRepository_AndReturnRole()
        {
            var role = new RoleCreateRequest { Name = "Developer" };

            _repoMock.Setup(r => r.Create(It.IsAny<Role>()))
                .ReturnsAsync((Role r) =>
                {
                    r.Id = Guid.NewGuid();
                    r.CreatedDate = DateTime.UtcNow;
                    r.UserUpdated = "system";
                    return r;
                });

            var result = await _service.Create(role);

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("Developer", result.Name);
            _repoMock.Verify(r => r.Create(It.IsAny<Role>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallRepository_AndReturnUpdatedRole()
        {
            var roleRequest = new RoleUpdateRequest { Id = Guid.NewGuid(), Name = "Tester" };

            var role = new Role()
            {
                Id = roleRequest.Id,
                Name = "Quality"
            };

            _repoMock.Setup(r => r.GetById(roleRequest.Id)).ReturnsAsync(role);
            _repoMock.Setup(r => r.Update(role)).ReturnsAsync(role);

            var result = await _service.Update(roleRequest);

            Assert.Equal("Tester", result.Name);
            _repoMock.Verify(r => r.Update(role), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenSuccess()
        {
            _repoMock.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync(1);

            var result = await _service.Delete(Guid.NewGuid());

            Assert.True(result);
        }

        [Fact]
        public async Task GetById_ShouldReturnRole()
        {
            var id = Guid.NewGuid();
            var expected = new Role { Id = id, Name = "Manager" };

            _repoMock.Setup(r => r.GetById(id)).ReturnsAsync(expected);

            var result = await _service.GetById(id);

            Assert.Equal("Manager", result.Name);
        }

        [Fact]
        public async Task GetAll_ShouldReturnRoles()
        {
            _repoMock.Setup(r => r.GetAll()).ReturnsAsync(
            [
                new Role { Name = "Developer" },
                new Role { Name = "Analyst" }
            ]);

            var result = await _service.GetAll();

            Assert.Equal(2, result.Count());
        }
    }
}
