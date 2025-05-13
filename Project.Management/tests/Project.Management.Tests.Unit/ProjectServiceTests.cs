using Moq;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Projects;

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
            _service = new ProjectService(_notifMock.Object, _repoMock.Object);
        }

        [Fact]
        public async Task Create_ShouldCallRepository()
        {
            var project = new Domain.Entities.Project { Name = "Test", Status = Domain.Enums.ProjectStatus.InProgress, StartDate = DateTime.UtcNow };
            _repoMock.Setup(r => r.Create(project)).ReturnsAsync(project);

            var result = await _service.Create(project);

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
    }

}
