using Project.Management.Domain.Entities;

namespace Project.Management.Infrastructure.Data
{
    public partial class ProjectManagementDbContext
    {
        private const string User = "system";

        public void SeedData()
        {
            if (!ProjectMembers.Any())
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
                    Id = Guid.Parse("7e46bcbb-95b7-4e14-bd94-6a657458ce5e"),
                    Name = "Developer",
                    CreatedDate = DateTime.UtcNow,
                    UserUpdated = User
                };

                var roleMgr = new Role
                {
                    Id = Guid.Parse("2fca76c1-18dd-486a-aa46-c1edf17f6a4c"),
                    Name = "Manager",
                    CreatedDate = DateTime.UtcNow,
                    UserUpdated = User
                };

                Roles.AddRange(roleDev, roleMgr);

                var project1 = new Domain.Entities.Project
                {
                    Id = Guid.Parse("49b0c5ca-5108-4f21-b4d7-a8d2c4cc99cd"),
                    Name = "Project Alpha",
                    Description = "API for client X",
                    CreatedDate = DateTime.UtcNow,
                    UserUpdated = User
                };

                Projects.Add(project1);

                var task1 = new TaskItem
                {
                    Id = Guid.Parse("6608302c-9b26-4a8e-aadb-88e824a6ca5c"),
                    Title = "Create API endpoints",
                    Description = "Implement CRUD",
                    ProjectId = project1.Id,
                    AssignedUserId = user1.Id,
                    Status = Domain.Enums.TaskState.New,
                    CreatedDate = DateTime.UtcNow,
                    UserUpdated = User
                };

                Tasks.Add(task1);

                var member1 = new ProjectMember
                {
                    Id = Guid.Parse("c5082695-e1ed-4b21-9386-f53d6ba5278e"),
                    ProjectId = project1.Id,
                    UserId = user1.Id,
                    RoleId = roleDev.Id,
                    Active = true
                };

                ProjectMembers.Add(member1);

                var member2 = new ProjectMember
                {
                    Id = Guid.Parse("3c463de9-b0ca-430a-a1a7-35da89355a69"),
                    ProjectId = project1.Id,
                    UserId = user2.Id,
                    RoleId = roleMgr.Id,
                    Active = true
                };

                ProjectMembers.Add(member2);

                SaveChanges();
            }
        }
    }
}
