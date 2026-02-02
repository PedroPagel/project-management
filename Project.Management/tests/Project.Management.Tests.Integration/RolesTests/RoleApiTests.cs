using Project.Management.Api.Dtos;

namespace Project.Management.Tests.Integration.RolesTests
{
    [CollectionDefinition("Integration Tests", DisableParallelization = true)]
    public class RoleApiTests(TestApiFixture fixture) : BaseApiTests(fixture)
    {
        [Fact]
        public async Task GetRoles_ReturnsSeededRoles()
        {
            // Act
            var roles = await GetAsync<IEnumerable<RoleDto>>("/api/role/all-roles");

            // Assert
            Assert.NotEmpty(roles);
        }

        [Fact]
        public async Task GetRoleById_ReturnsSeededRole()
        {
            // Arrange
            var roleId = Guid.Parse("7e46bcbb-95b7-4e14-bd94-6a657458ce5e");

            // Act
            var role = await GetAsync<RoleDto>($"/api/role/role-by-id/{roleId}");

            // Assert
            Assert.Equal("Developer", role.Name);
        }

        [Fact]
        public async Task CreateRole_CanDeleteRole()
        {
            // Arrange
            var request = new
            {
                name = $"QA {Guid.NewGuid():N}"
            };

            // Act
            var createdRole = await PostAsync<RoleDto>("/api/role/add", request);
            var deleted = await DeleteAsync<bool>($"/api/role/delete-by-id/{createdRole.Id}");

            // Assert
            Assert.Equal(request.name, createdRole.Name);
            Assert.True(deleted);
        }
    }
}
