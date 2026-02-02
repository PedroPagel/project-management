using Project.Management.Api.Dtos;

namespace Project.Management.Tests.Integration.ProjectsTests
{
    [CollectionDefinition("Integration Tests", DisableParallelization = true)]
    public class ProjectApiTests(TestApiFixture fixture) : BaseApiTests(fixture)
    {
        [Fact]
        public async Task GetProjects_ReturnsSeededProjects()
        {
            // Act
            var projects = await GetAsync<IEnumerable<ProjectDto>>("/api/project/all-projects");

            // Assert
            Assert.NotEmpty(projects);
        }

        [Fact]
        public async Task GetProjectById_ReturnsSeededProject()
        {
            // Arrange
            var projectId = Guid.Parse("49b0c5ca-5108-4f21-b4d7-a8d2c4cc99cd");

            // Act
            var project = await GetAsync<ProjectDto>($"/api/project/project-by-id/{projectId}");

            // Assert
            Assert.Equal("Project Alpha", project.Name);
        }

        [Fact]
        public async Task CreateProject_ReturnsCreatedProject()
        {
            // Arrange
            var request = new
            {
                name = $"Project Beta {Guid.NewGuid():N}",
                description = "Integration test project",
                startDate = DateTime.UtcNow.Date,
                endDate = DateTime.UtcNow.Date.AddDays(14)
            };

            // Act
            var project = await PostAsync<ProjectDto>("/api/project/create", request);

            // Assert
            Assert.Equal(request.name, project.Name);
            Assert.Equal(request.description, project.Description);
        }
    }
}
