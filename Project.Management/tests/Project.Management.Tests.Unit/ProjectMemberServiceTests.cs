using Microsoft.Extensions.Logging;
using Moq;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Projects;
using Project.Management.Domain.Services.Projects.Models;

namespace Project.Management.Tests.Unit
{
    public class ProjectMemberServiceTests
    {
        private readonly Mock<IProjectMemberRepository> _repoMock;
        private readonly Mock<INotificator> _notifMock;
        private readonly ProjectMemberService _service;

        public ProjectMemberServiceTests()
        {
            _repoMock = new Mock<IProjectMemberRepository>();
            _notifMock = new Mock<INotificator>();
            _service = new ProjectMemberService(
                _notifMock.Object,
                _repoMock.Object,
                new Mock<ILogger<ProjectMemberService>>().Object);
        }

        private static ProjectMemberCreationRequest ValidRequest() => new()
        {
            UserId = Guid.NewGuid(),
            RoleId = Guid.NewGuid(),
            ProjectId = Guid.NewGuid()
        };

        [Fact]
        public async Task Create_ShouldCallRepository_WhenValid()
        {
            var request = ValidRequest();

            _repoMock.Setup(r => r.GetProjectMemberLink(request))
                .ReturnsAsync(new ProjectMemberQueryValidation
                {
                    UserExists = true,
                    RoleExists = true,
                    ProjectExists = true,
                    ProjectMemberId = Guid.Empty
                });

            _repoMock.Setup(r => r.Create(It.IsAny<ProjectMember>()))
                .ReturnsAsync((ProjectMember pm) =>
                {
                    pm.Id = Guid.NewGuid();
                    return pm;
                });

            var result = await _service.Create(request);

            Assert.NotNull(result);
            Assert.Equal(request.UserId, result.UserId);
            _repoMock.Verify(r => r.Create(It.IsAny<ProjectMember>()), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnNull_WhenUserNotFound()
        {
            var request = ValidRequest();

            _repoMock.Setup(r => r.GetProjectMemberLink(request))
                .ReturnsAsync(new ProjectMemberQueryValidation
                {
                    UserExists = false
                });

            var result = await _service.Create(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.Create(It.IsAny<ProjectMember>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldReturnNull_WhenRoleNotFound()
        {
            var request = ValidRequest();

            _repoMock.Setup(r => r.GetProjectMemberLink(request))
                .ReturnsAsync(new ProjectMemberQueryValidation
                {
                    UserExists = true,
                    RoleExists = false,
                    ProjectExists = true
                });

            var result = await _service.Create(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.Create(It.IsAny<ProjectMember>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldReturnNull_WhenProjectNotFound()
        {
            var request = ValidRequest();

            _repoMock.Setup(r => r.GetProjectMemberLink(request))
                .ReturnsAsync(new ProjectMemberQueryValidation
                {
                    UserExists = true,
                    RoleExists = true,
                    ProjectExists = false
                });

            var result = await _service.Create(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.Create(It.IsAny<ProjectMember>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldReturnNull_WhenProjectMemberAlreadyExists()
        {
            var request = ValidRequest();

            _repoMock.Setup(r => r.GetProjectMemberLink(request))
                .ReturnsAsync(new ProjectMemberQueryValidation
                {
                    UserExists = true,
                    RoleExists = true,
                    ProjectExists = true,
                    ProjectMemberId = Guid.NewGuid()
                });

            var result = await _service.Create(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.Create(It.IsAny<ProjectMember>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldReturnNull_WhenValidationFails()
        {
            var request = new ProjectMemberCreationRequest
            {
                UserId = Guid.Empty,
                RoleId = Guid.Empty,
                ProjectId = Guid.Empty
            };

            var result = await _service.Create(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.GetProjectMemberLink(It.IsAny<ProjectMemberCreationRequest>()), Times.Never);
            _repoMock.Verify(r => r.Create(It.IsAny<ProjectMember>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldReturnNull_WhenNotFound()
        {
            var request = new ProjectMemberUpdateRequest
            {
                ProjectMemberId = Guid.NewGuid()
            };

            _repoMock.Setup(r => r.GetById(request.ProjectMemberId))
                .ReturnsAsync((ProjectMember)null!);

            var result = await _service.Update(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.Update(It.IsAny<ProjectMember>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldCallRepository_WhenValid()
        {
            var member = new ProjectMember
            {
                Id = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Active = true
            };

            var request = new ProjectMemberUpdateRequest
            {
                ProjectMemberId = member.Id,
                RoleId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Active = false
            };

            _repoMock.Setup(r => r.GetById(member.Id)).ReturnsAsync(member);
            _repoMock.Setup(r => r.Update(member)).ReturnsAsync(member);

            var result = await _service.Update(request);

            Assert.NotNull(result);
            Assert.Equal(request.RoleId, result.RoleId);
            Assert.False(result.Active);
            _repoMock.Verify(r => r.Update(member), Times.Once);
        }

        [Fact]
        public async Task GetProjectMember_ShouldCallRepository()
        {
            var request = new ProjectMemberRequest
            {
                UserId = Guid.NewGuid()
            };

            _repoMock.Setup(r => r.Get(It.IsAny<System.Linq.Expressions.Expression<Func<ProjectMember, bool>>>()))
                .ReturnsAsync(new List<ProjectMember> { new ProjectMember() });

            var result = await _service.GetProjectMember(request);

            Assert.Single(result);
            _repoMock.Verify(r => r.Get(It.IsAny<System.Linq.Expressions.Expression<Func<ProjectMember, bool>>>()), Times.Once);
        }
    }
}
