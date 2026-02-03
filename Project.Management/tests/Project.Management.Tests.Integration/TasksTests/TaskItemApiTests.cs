using Project.Management.Api.Dtos;
using Project.Management.Domain.Enums;

namespace Project.Management.Tests.Integration.TasksTests
{
    [CollectionDefinition("Integration Tests", DisableParallelization = true)]
    public class TaskItemApiTests(TestApiFixture fixture) : BaseApiTests(fixture)
    {
        [Fact]
        public async Task GetTasks_ReturnsSeededTasks()
        {
            // Act
            var tasks = await GetAsync<IEnumerable<TaskItemDto>>("/api/task/all-tasks");

            // Assert
            Assert.NotEmpty(tasks);
        }

        [Fact]
        public async Task GetTaskById_ReturnsSeededTask()
        {
            // Arrange
            var taskId = Guid.Parse("6608302c-9b26-4a8e-aadb-88e824a6ca5c");

            // Act
            var task = await GetAsync<TaskItemDto>($"/api/task/task-by-id/{taskId}");

            // Assert
            Assert.Equal("Kickoff meeting", task.Title);
        }

        [Fact]
        public async Task CreateTask_ReturnsCreatedTask()
        {
            // Arrange
            var request = new
            {
                projectId = Guid.Parse("49b0c5ca-5108-4f21-b4d7-a8d2c4cc99cd"),
                assignedUserId = Guid.Parse("3f9a5f80-3f3e-4e6c-9d91-2c5d7aeb73d4"),
                title = $"Integration Task {Guid.NewGuid():N}",
                description = "Integration task description",
                dueDate = DateTime.UtcNow.AddDays(5)
            };

            // Act
            var task = await PostAsync<TaskItemDto>("/api/task/add", request);

            // Assert
            Assert.Equal(request.title, task.Title);
            Assert.Equal(request.projectId, task.ProjectId);
        }

        [Fact]
        public async Task UpdateTask_ChangesDetails()
        {
            // Arrange
            var createRequest = new
            {
                projectId = Guid.Parse("49b0c5ca-5108-4f21-b4d7-a8d2c4cc99cd"),
                assignedUserId = Guid.Parse("3f9a5f80-3f3e-4e6c-9d91-2c5d7aeb73d4"),
                title = $"Integration Update {Guid.NewGuid():N}",
                description = "Initial description",
                dueDate = DateTime.UtcNow.AddDays(3)
            };

            var createdTask = await PostAsync<TaskItemDto>("/api/task/add", createRequest);

            var updateRequest = new
            {
                taskId = createdTask.Id,
                status = TaskState.InProgress,
                title = "Updated title",
                description = "Updated description",
                dueDate = DateTime.UtcNow.AddDays(10)
            };

            // Act
            var updatedTask = await PutAsync<TaskItemDto>("/api/task/update", updateRequest);

            // Assert
            Assert.Equal(updateRequest.title, updatedTask.Title);
            Assert.Equal(updateRequest.description, updatedTask.Description);
        }
    }
}
