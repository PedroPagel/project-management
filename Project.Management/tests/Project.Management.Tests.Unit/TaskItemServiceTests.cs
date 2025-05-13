using Moq;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Tasks;

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
            _service = new TaskItemService(_notificatorMock.Object, _repoMock.Object);
        }

        [Fact]
        public async Task Create_ShouldCallRepository_AndReturnCreatedItem()
        {
            var taskItem = new TaskItem { Title = "Fix Bug", Description = "Fix issue #123" };

            _repoMock.Setup(r => r.Create(It.IsAny<TaskItem>()))
                .ReturnsAsync((TaskItem t) =>
                {
                    t.Id = Guid.NewGuid();
                    t.CreatedDate = DateTime.UtcNow;
                    t.UserUpdated = "system";
                    return t;
                });

            var result = await _service.Create(taskItem);

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal("system", result.UserUpdated);
            _repoMock.Verify(r => r.Create(It.IsAny<TaskItem>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallRepository()
        {
            var taskItem = new TaskItem { Id = Guid.NewGuid(), Title = "Update Task" };

            _repoMock.Setup(r => r.Update(taskItem)).ReturnsAsync(taskItem);

            var result = await _service.Update(taskItem);

            Assert.Equal(taskItem.Title, result.Title);
            _repoMock.Verify(r => r.Update(taskItem), Times.Once);
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
    }

}
