using Microsoft.Extensions.Logging;
using Moq;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Projects;
using Project.Management.Domain.Services.Projects.Models;

namespace Project.Management.Tests.Unit
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _repoMock;
        private readonly Mock<INotificator> _notifMock;
        private readonly ProjectService _service;

        public ProjectServiceTests()
        {
            _repoMock = new Mock<IProjectRepository>();
            _notifMock = new Mock<INotificator>();
            _service = new ProjectService(_notifMock.Object, _repoMock.Object, new Mock<ILogger<ProjectService>>().Object);
        }

        [Fact]
        public async Task Create_ShouldCallRepository()
        {
            var request = new ProjectCreationRequest 
            { 
                Name = "Test", 
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddYears(1),
                Description = "DescTest"
            };

            _repoMock.Setup(r => r.Create(It.IsAny<Domain.Entities.Project>()))
                .ReturnsAsync((Domain.Entities.Project p) =>
                {
                    p.Id = Guid.NewGuid();
                    p.CreatedDate = DateTime.UtcNow;
                    p.UserUpdated = "system";
                    p.Name = "Test";
                    p.Status = Domain.Enums.ProjectStatus.Planned;

                    return p;
                });

            var result = await _service.Create(request);

            Assert.Equal("Test", result.Name);
            _repoMock.Verify(r => r.Create(It.IsAny<Domain.Entities.Project>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenDeleted()
        {
            _repoMock.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync(1);

            var result = await _service.Delete(Guid.NewGuid());

            Assert.True(result);
        }

        [Fact]
        public async Task Create_ShouldReturnNull_WhenValidationFails()
        {
            var request = new ProjectCreationRequest
            {
                Name = "",
                StartDate = DateTime.UtcNow.AddDays(-2),
                EndDate = DateTime.UtcNow.AddDays(-1),
                Description = "bad"
            };

            var result = await _service.Create(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.Create(It.IsAny<Domain.Entities.Project>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldReturnNull_WhenNameAlreadyExists()
        {
            var request = new ProjectCreationRequest
            {
                Name = "Existing",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10),
                Description = "Existing project"
            };

            _repoMock.Setup(r => r.FirstOrDefault(It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.Project, bool>>>()))
                .ReturnsAsync(new Domain.Entities.Project { Id = Guid.NewGuid(), Name = request.Name });

            var result = await _service.Create(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.Create(It.IsAny<Domain.Entities.Project>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldReturnNull_WhenIdIsEmpty()
        {
            var request = new ProjectUpdateRequest
            {
                Id = Guid.Empty,
                Name = "Updated",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            var result = await _service.Update(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.Update(It.IsAny<Domain.Entities.Project>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldReturnNull_WhenProjectNotFound()
        {
            var request = new ProjectUpdateRequest
            {
                Id = Guid.NewGuid(),
                Name = "Updated",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            _repoMock.Setup(r => r.GetById(request.Id)).ReturnsAsync((Domain.Entities.Project)null);

            var result = await _service.Update(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.Update(It.IsAny<Domain.Entities.Project>()), Times.Never);
        }
    }

}
