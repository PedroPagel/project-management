using Microsoft.Extensions.Logging;
using Moq;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Tasks;
using Project.Management.Domain.Services.Tasks.Models;

namespace Project.Management.Tests.Unit
{
    public class TaskItemServiceTests
    {
        private readonly Mock<ITaskItemRepository> _repoMock;
        private readonly Mock<INotificator> _notificatorMock;
        private readonly TaskItemService _service;

        public TaskItemServiceTests()
        {
            _repoMock = new Mock<ITaskItemRepository>();
            _notificatorMock = new Mock<INotificator>();
            _service = new TaskItemService(_notificatorMock.Object, _repoMock.Object, new Mock<ILogger<TaskItemService>>().Object);
        }

        [Fact]
        public async Task Create_ShouldCallRepository_AndReturnCreatedItem()
        {
            var taskItem = new TaskItemCreationRequest
            {
                Title = "Fix Bug",
                Description = "Fix issue #123",
                AssignedUserId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid(),
                DueDate = DateTime.UtcNow
            };

            _repoMock.Setup(r => r.Create(It.IsAny<TaskItem>()))
                .ReturnsAsync((TaskItem t) =>
                {
                    t.Id = Guid.NewGuid();
                    t.CreatedDate = DateTime.UtcNow;
                    t.UserUpdated = "system";
                    t.ProjectId = Guid.NewGuid();

                    return t;
                });

            var result = await _service.Create(taskItem);

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("system", result.UserUpdated);
            _repoMock.Verify(r => r.Create(It.IsAny<TaskItem>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallRepository()
        {
            var id = Guid.NewGuid();
            var expected = new TaskItem { Id = id, Title = "Test" };

            var taskItem = new TaskItemUpdateRequest
            {

                Status = Domain.Enums.TaskState.Done,
                Title = "Update Task",
                TaskId = id,
            };

            _repoMock.Setup(r => r.Update(It.IsAny<TaskItem>()))
                .ReturnsAsync((TaskItem t) =>
                {
                    t.Id = Guid.NewGuid();
                    t.CreatedDate = DateTime.UtcNow;
                    t.UserUpdated = "system";
                    return t;
                });

            _repoMock.Setup(r => r.GetById(id)).ReturnsAsync(expected);

            var result = await _service.Update(taskItem);

            Assert.Equal(taskItem.Title, result.Title);
            _repoMock.Verify(r => r.Update(It.IsAny<TaskItem>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenRepositoryReturnsPositiveResult()
        {
            _repoMock.Setup(r => r.Delete(It.IsAny<Guid>())).ReturnsAsync(1);

            var result = await _service.Delete(Guid.NewGuid());

            Assert.True(result);
        }

        [Fact]
        public async Task GetById_ShouldReturnCorrectTaskItem()
        {
            var id = Guid.NewGuid();
            var expected = new TaskItem { Id = id, Title = "Test" };

            _repoMock.Setup(r => r.GetById(id)).ReturnsAsync(expected);

            var result = await _service.GetById(id);

            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfTaskItems()
        {
            _repoMock.Setup(static r => r.GetAll()).ReturnsAsync(
            [
                new() { Title = "Task 1" },
                new() { Title = "Task 2" }
            ]);

            var result = await _service.GetAll();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task Create_ShouldReturnNull_WhenValidationFails()
        {
            var taskItem = new TaskItemCreationRequest
            {
                Title = "",
                Description = "Missing title",
                AssignedUserId = Guid.Empty,
                ProjectId = Guid.Empty,
                DueDate = default
            };

            var result = await _service.Create(taskItem);

            Assert.Null(result);
            _repoMock.Verify(r => r.Create(It.IsAny<TaskItem>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldReturnNull_WhenTaskNotFound()
        {
            var request = new TaskItemUpdateRequest
            {
                TaskId = Guid.NewGuid(),
                Status = Domain.Enums.TaskState.InProgress,
                Title = "Update Task"
            };

            _repoMock.Setup(r => r.GetById(request.TaskId)).ReturnsAsync((TaskItem)null!);

            var result = await _service.Update(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.Update(It.IsAny<TaskItem>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldReturnNull_WhenValidationFails()
        {
            var taskId = Guid.NewGuid();
            var existing = new TaskItem { Id = taskId, Status = Domain.Enums.TaskState.Done };

            var request = new TaskItemUpdateRequest
            {
                TaskId = taskId,
                Status = Domain.Enums.TaskState.New,
                Title = "Invalid Status"
            };

            _repoMock.Setup(r => r.GetById(taskId)).ReturnsAsync(existing);

            var result = await _service.Update(request);

            Assert.Null(result);
            _repoMock.Verify(r => r.Update(It.IsAny<TaskItem>()), Times.Never);
        }
    }

}
