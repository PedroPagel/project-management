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
    }

}
