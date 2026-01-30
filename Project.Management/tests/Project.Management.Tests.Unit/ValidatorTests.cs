using Project.Management.Domain.Entities;
using Project.Management.Domain.Services.Projects.Models;
using Project.Management.Domain.Services.Projects.Validators;
using Project.Management.Domain.Services.Roles.Models;
using Project.Management.Domain.Services.Roles.Validators;
using Project.Management.Domain.Services.Tasks.Models;
using Project.Management.Domain.Services.Tasks.Validations;
using Project.Management.Domain.Services.Users.Models;
using Project.Management.Domain.Services.Users.Validators;

namespace Project.Management.Tests.Unit
{
    public class ValidatorTests
    {
        [Fact]
        public void UserCreationValidator_ShouldFail_WhenRequiredFieldsMissing()
        {
            var validator = new UserCreationValidator();
            var request = new UserCreationRequest { FullName = "", Email = "invalid", PasswordHash = "123" };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void UserCreationValidator_ShouldPass_WhenValid()
        {
            var validator = new UserCreationValidator();
            var request = new UserCreationRequest { FullName = "Alice", Email = "alice@mail.com", PasswordHash = "123456" };

            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void UserUpdateValidator_ShouldFail_WhenInvalidEmailProvided()
        {
            var validator = new UserUpdateValidator();
            var request = new UserUpdateRequest { Id = Guid.NewGuid(), Email = "invalid" };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void UserUpdateValidator_ShouldPass_WhenNoChangesProvided()
        {
            var validator = new UserUpdateValidator();
            var request = new UserUpdateRequest { Id = Guid.NewGuid() };

            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ProjectCreationValidator_ShouldFail_WhenDatesInvalid()
        {
            var validator = new ProjectCreationValidator();
            var request = new ProjectCreationRequest
            {
                Name = "Project",
                Description = "Short",
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow.AddDays(-2)
            };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void ProjectCreationValidator_ShouldPass_WhenValid()
        {
            var validator = new ProjectCreationValidator();
            var request = new ProjectCreationRequest
            {
                Name = "Project",
                Description = "Project description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(2)
            };

            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ProjectUpdateValidator_ShouldFail_WhenNameMatchesExisting()
        {
            var project = new Project.Management.Domain.Entities.Project
            {
                Id = Guid.NewGuid(),
                Name = "Existing",
                Description = "Description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            };
            var validator = new ProjectUpdateValidator(project);
            var request = new ProjectUpdateRequest
            {
                Id = project.Id,
                Name = "Existing",
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate
            };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void ProjectUpdateValidator_ShouldPass_WhenChangesValid()
        {
            var project = new Project.Management.Domain.Entities.Project
            {
                Id = Guid.NewGuid(),
                Name = "Existing",
                Description = "Description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            };
            var validator = new ProjectUpdateValidator(project);
            var request = new ProjectUpdateRequest
            {
                Id = project.Id,
                Name = "Updated",
                Description = "Updated description",
                StartDate = project.StartDate.AddDays(1),
                EndDate = project.EndDate.AddDays(2)
            };

            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ProjectMemberCreationValidation_ShouldFail_WhenIdsMissing()
        {
            var validator = new ProjectMemberCreationValidation();
            var request = new ProjectMemberCreationRequest
            {
                UserId = Guid.Empty,
                RoleId = Guid.Empty,
                ProjectId = Guid.Empty
            };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void ProjectMemberCreationValidation_ShouldPass_WhenValid()
        {
            var validator = new ProjectMemberCreationValidation();
            var request = new ProjectMemberCreationRequest
            {
                UserId = Guid.NewGuid(),
                RoleId = Guid.NewGuid(),
                ProjectId = Guid.NewGuid()
            };

            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void RoleCreationValidator_ShouldFail_WhenNameTooShort()
        {
            var validator = new RoleCreationValidator();
            var request = new RoleCreateRequest { Name = "De" };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void RoleCreationValidator_ShouldPass_WhenValid()
        {
            var validator = new RoleCreationValidator();
            var request = new RoleCreateRequest { Name = "Developer" };

            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void RoleUpdateValidator_ShouldFail_WhenNameMatchesExisting()
        {
            var role = new Role { Id = Guid.NewGuid(), Name = "Analyst" };
            var validator = new RoleUpdateValidator(role);
            var request = new RoleUpdateRequest { Id = role.Id, Name = "Analyst" };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void RoleUpdateValidator_ShouldPass_WhenValid()
        {
            var role = new Role { Id = Guid.NewGuid(), Name = "Analyst" };
            var validator = new RoleUpdateValidator(role);
            var request = new RoleUpdateRequest { Id = role.Id, Name = "Manager" };

            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void TaskItemCreationValidation_ShouldFail_WhenMissingFields()
        {
            var validator = new TaskItemCreationValidation();
            var request = new TaskItemCreationRequest
            {
                ProjectId = Guid.Empty,
                Title = "",
                AssignedUserId = Guid.Empty,
                DueDate = default
            };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void TaskItemCreationValidation_ShouldPass_WhenValid()
        {
            var validator = new TaskItemCreationValidation();
            var request = new TaskItemCreationRequest
            {
                ProjectId = Guid.NewGuid(),
                Title = "Task",
                AssignedUserId = Guid.NewGuid(),
                DueDate = DateTime.UtcNow
            };

            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void TaskItemUpdateValidation_ShouldFail_WhenStatusInvalid()
        {
            var validator = new TaskItemUpdateValidation(Domain.Enums.TaskState.Done);
            var request = new TaskItemUpdateRequest
            {
                TaskId = Guid.NewGuid(),
                Status = Domain.Enums.TaskState.New
            };

            var result = validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void TaskItemUpdateValidation_ShouldPass_WhenStatusValid()
        {
            var validator = new TaskItemUpdateValidation(Domain.Enums.TaskState.New);
            var request = new TaskItemUpdateRequest
            {
                TaskId = Guid.NewGuid(),
                Status = Domain.Enums.TaskState.InProgress
            };

            var result = validator.Validate(request);

            Assert.True(result.IsValid);
        }
    }
}
