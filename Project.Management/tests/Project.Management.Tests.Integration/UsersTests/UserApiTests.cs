using Project.Management.Api.Dtos;

namespace Project.Management.Tests.Integration.UsersTests
{
    public class UserApiTests(TestApiFixture fixture) : BaseApiTests(fixture)
    {
        [Fact]
        public async Task PostUser_ShouldReturnCreated()
        {
            // Arrange
            var newUser = new
            {
                fullName = "Maria",
                email = "maria@example.com",
                passwordHash = "123456"
            };

            // Act
            var content = await PostAsync<UserDto>("/api/user/add", newUser);

            // Assert
            Assert.True(content.Email.Equals(newUser.email));
        }

        [Fact]
        public async Task UpdateUSer_ShouldReturn_Update()
        {
            // Arrange
            var userUpdate = new
            {
                id = Guid.Parse("1e29e389-abdc-4de7-ad7d-52e094724652"),
                fullName = "John",
                email = "john@example.com",
                passwordHash = "123456"
            };

            // Act
            var content = await PutAsync<UserDto>("/api/user/update", userUpdate);

            // Assert
            Assert.True(content.Email.Equals(userUpdate.email));
        }

        [Fact]
        public async Task GetUser_NotValid_Return_BadRequest()
        {
            // Arrange
            var url = $"/api/user/user-by-id/{Guid.Empty}";

            // Act
            var content = await GetAsync<string>(url);

            // Assert
            Assert.Contains("Invalid user Id provided", content);
        }

        [Fact]
        public async Task GetUser_NotValid_Return_NotFound()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act
            var content = await GetAsync<string>($"/api/user/user-by-id/{guid}");

            // Assert
            Assert.Contains("User not found", content);
        }

        [Fact]
        public async Task GetUser_ValidUser_Return_Ok()
        {
            // Arrange
            var url = $"/api/user/user-by-id/{Guid.Parse("3f9a5f80-3f3e-4e6c-9d91-2c5d7aeb73d4")}";

            // Act
            var content = await GetAsync<UserDto>(url);

            // Assert
            Assert.True(content.FullName.Equals("Alice Doe"));
        }

        [Fact]
        public async Task GetUser_AllUsers_Return_Ok()
        {
            // Arrange
            var url = "/api/user/all-users";

            // Act
            var content = await GetAsync<IEnumerable<UserDto>>(url);

            // Assert
            Assert.NotEmpty(content);
        }

        [Fact]
        public async Task Delete_ValidUser_Return_True()
        {
            // Arrange
            var url = $"/api/user/delete-by-id/{Guid.Parse("b708f42c-83a8-4855-a5a9-6bedb27cdadd")}";

            // Act
            var content = await DeleteAsync<bool>(url);

            // Assert
            Assert.True(content);
        }
    }
}
