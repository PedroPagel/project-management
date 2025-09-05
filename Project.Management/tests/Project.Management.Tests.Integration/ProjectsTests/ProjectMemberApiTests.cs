using Project.Management.Api.Dtos;

namespace Project.Management.Tests.Integration.ProjectsTests
{
    [CollectionDefinition("Integration Tests", DisableParallelization = true)]
    public class ProjectMemberApiTests(TestApiFixture fixture) : BaseApiTests(fixture)
    {
        [Fact]
        public async Task PostProjectMember_ShouldReturnCreated()
        {
            // Arrange
            var projectMember = new
            {
                projectId = Guid.Parse("49b0c5ca-5108-4f21-b4d7-a8d2c4cc99cd"),
                roleId = Guid.Parse("7e46bcbb-95b7-4e14-bd94-6a657458ce5e"),
                userId = Guid.Parse("1e29e389-abdc-4de7-ad7d-52e094724652")
            };

            // Act
            var content = await PostAsync<ProjectMemberDto>("/api/project-member/create", projectMember);

            // Assert
            Assert.True(content.UserId == projectMember.userId);
            Assert.True(content.ProjectId == projectMember.projectId);
            Assert.True(content.RoleId == projectMember.roleId);
        }

        [Fact]
        public async Task PostProjectMember_ShouldReturnUpdated()
        {
            // Arrange
            var projectMember = new
            {
                projectMemberId = Guid.Parse("c5082695-e1ed-4b21-9386-f53d6ba5278e"),
                active = false
            };

            // Act
            var content = await PostAsync<ProjectMemberDto>("/api/project-member/update", projectMember);

            // Assert
            Assert.True(!content.Active);
        }

        [Fact]
        public async Task PostProjectMember_ShouldReturnProjectMembrs()
        {
            // Arrange
            var projectMember = new
            {
                projectId = Guid.Parse("49b0c5ca-5108-4f21-b4d7-a8d2c4cc99cd")
            };

            // Act
            var content = await GetAsync<IEnumerable<ProjectMemberDto>>("/api/project-member/projects-members", projectMember);

            // Assert
            Assert.True(content.Count() > 1);
        }
    }
}
