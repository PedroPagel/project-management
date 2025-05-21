using Project.Management.Domain.Entities;

namespace Project.Management.Infrastructure.Data
{
    public partial class ProjectManagementDbContext
    {
        private const string User = "system";

        public void SeedData()
        {
            if (!Users.Any())
            {
                var user1 = new User
                {
                    Id = Guid.Parse("3f9a5f80-3f3e-4e6c-9d91-2c5d7aeb73d4"),
                    FullName = "Alice Doe",
                    Email = "alice@example.com",
                    PasswordHash = "hash1",
                    CreatedDate = DateTime.UtcNow,
                    UserUpdated = User
                };

                var user2 = new User
                {
                    Id = Guid.Parse("b708f42c-83a8-4855-a5a9-6bedb27cdadd"),
                    FullName = "Bob Smith",
                    Email = "bob@example.com",
                    PasswordHash = "hash2",
                    CreatedDate = DateTime.UtcNow,
                    UserUpdated = User
                };

                var user3 = new User
                {
                    Id = Guid.Parse("1e29e389-abdc-4de7-ad7d-52e094724652"),
                    FullName = "Joey",
                    Email = "joey@example.com",
                    PasswordHash = "hash3",
                    CreatedDate = DateTime.UtcNow,
                    UserUpdated = User
                };

                Users.AddRange(user1, user2, user3);

                var roleDev = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Developer",
                    CreatedDate = DateTime.UtcNow,
                    UserUpdated = User
                };

                var roleMgr = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Manager",
                    CreatedDate = DateTime.UtcNow,
                    UserUpdated = User
                };

                Roles.AddRange(roleDev, roleMgr);

                var project1 = new Domain.Entities.Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Project Alpha",
                    Description = "API for client X",
                    CreatedDate = DateTime.UtcNow,
                    UserUpdated = User
                };

                Projects.Add(project1);

                var task1 = new TaskItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Create API endpoints",
                    Description = "Implement CRUD",
                    ProjectId = project1.Id,
                    AssignedUserId = user1.Id,
                    Status = TaskStatus.Created,
                    CreatedDate = DateTime.UtcNow,
                    UserUpdated = User
                };

                Tasks.Add(task1);

                var member1 = new ProjectMember
                {
                    ProjectId = project1.Id,
                    UserId = user1.Id,
                    RoleId = roleDev.Id
                };

                ProjectMembers.Add(member1);

                SaveChanges();
            }
        }
    }
}
