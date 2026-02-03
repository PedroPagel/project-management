using Project.Management.Domain.Entities;
using Project.Management.Domain.Enums;

namespace Project.Management.Infrastructure.Data
{
    public static class SeedDataFactory
    {
        public const string SystemUser = "system";

        public static readonly Guid UserAliceId = Guid.Parse("3f9a5f80-3f3e-4e6c-9d91-2c5d7aeb73d4");
        public static readonly Guid UserBobId = Guid.Parse("b708f42c-83a8-4855-a5a9-6bedb27cdadd");
        public static readonly Guid UserJoeyId = Guid.Parse("1e29e389-abdc-4de7-ad7d-52e094724652");
        public static readonly Guid UserEmmaId = Guid.Parse("fbe0c2ee-9d3d-4fa1-b44f-75f9a4be6ed4");

        public static readonly Guid RoleDeveloperId = Guid.Parse("7e46bcbb-95b7-4e14-bd94-6a657458ce5e");
        public static readonly Guid RoleManagerId = Guid.Parse("2fca76c1-18dd-486a-aa46-c1edf17f6a4c");
        public static readonly Guid RoleQaId = Guid.Parse("e0c0f1f8-8f44-4c78-9bd1-2d8bd0b96f9f");
        public static readonly Guid RoleProductId = Guid.Parse("5b3c8db0-6d6d-4b6e-9ce1-87c820f7f14c");

        public static readonly Guid ProjectAlphaId = Guid.Parse("49b0c5ca-5108-4f21-b4d7-a8d2c4cc99cd");
        public static readonly Guid ProjectBetaId = Guid.Parse("9f9d7c2c-0d6c-4c5b-9203-2bdb1b5c2b0f");
        public static readonly Guid ProjectGammaId = Guid.Parse("1a07c79b-3f90-4b1a-a4f4-1f2f6b7c07b4");

        public static IReadOnlyList<User> BuildUsers(DateTime now) =>
        [
            new User
            {
                Id = UserAliceId,
                FullName = "Alice Doe",
                Email = "alice@example.com",
                PasswordHash = "hash1",
                CreatedDate = now,
                UserUpdated = SystemUser
            },
            new User
            {
                Id = UserBobId,
                FullName = "Bob Smith",
                Email = "bob@example.com",
                PasswordHash = "hash2",
                CreatedDate = now,
                UserUpdated = SystemUser
            },
            new User
            {
                Id = UserJoeyId,
                FullName = "Joey Rivera",
                Email = "joey@example.com",
                PasswordHash = "hash3",
                CreatedDate = now,
                UserUpdated = SystemUser
            },
            new User
            {
                Id = UserEmmaId,
                FullName = "Emma Lee",
                Email = "emma@example.com",
                PasswordHash = "hash4",
                CreatedDate = now,
                UserUpdated = SystemUser
            }
        ];

        public static IReadOnlyList<Role> BuildRoles(DateTime now) =>
        [
            new Role
            {
                Id = RoleDeveloperId,
                Name = "Developer",
                CreatedDate = now,
                UserUpdated = SystemUser
            },
            new Role
            {
                Id = RoleManagerId,
                Name = "Manager",
                CreatedDate = now,
                UserUpdated = SystemUser
            },
            new Role
            {
                Id = RoleQaId,
                Name = "QA",
                CreatedDate = now,
                UserUpdated = SystemUser
            },
            new Role
            {
                Id = RoleProductId,
                Name = "Product Owner",
                CreatedDate = now,
                UserUpdated = SystemUser
            }
        ];

        public static IReadOnlyList<Domain.Entities.Project> BuildProjects(DateTime now) =>
        [
            new Domain.Entities.Project
            {
                Id = ProjectAlphaId,
                Name = "Project Alpha",
                Description = "API for client X",
                Status = ProjectStatus.InProgress,
                StartDate = now.AddDays(-7),
                CreatedDate = now.AddDays(-7),
                UserUpdated = SystemUser
            },
            new Domain.Entities.Project
            {
                Id = ProjectBetaId,
                Name = "Project Beta",
                Description = "Mobile app upgrade",
                Status = ProjectStatus.Planned,
                StartDate = now.AddDays(2),
                CreatedDate = now.AddDays(-1),
                UserUpdated = SystemUser
            },
            new Domain.Entities.Project
            {
                Id = ProjectGammaId,
                Name = "Project Gamma",
                Description = "Data warehouse migration",
                Status = ProjectStatus.OnHold,
                StartDate = now.AddDays(-30),
                CreatedDate = now.AddDays(-30),
                UserUpdated = SystemUser
            }
        ];

        public static IReadOnlyList<ProjectMember> BuildProjectMembers(DateTime now) =>
        [
            new ProjectMember
            {
                Id = Guid.Parse("c5082695-e1ed-4b21-9386-f53d6ba5278e"),
                ProjectId = ProjectAlphaId,
                UserId = UserAliceId,
                RoleId = RoleDeveloperId,
                Active = true,
                CreatedDate = now,
                UserUpdated = SystemUser
            },
            new ProjectMember
            {
                Id = Guid.Parse("3c463de9-b0ca-430a-a1a7-35da89355a69"),
                ProjectId = ProjectAlphaId,
                UserId = UserBobId,
                RoleId = RoleManagerId,
                Active = true,
                CreatedDate = now,
                UserUpdated = SystemUser
            },
            new ProjectMember
            {
                Id = Guid.Parse("3b4b52bb-7d60-4d74-9d6e-8b9f4d86a6a1"),
                ProjectId = ProjectBetaId,
                UserId = UserEmmaId,
                RoleId = RoleProductId,
                Active = true,
                CreatedDate = now,
                UserUpdated = SystemUser
            },
            new ProjectMember
            {
                Id = Guid.Parse("9b6cb9d4-4c1a-4d7d-9b89-18f8fa86b1ee"),
                ProjectId = ProjectBetaId,
                UserId = UserJoeyId,
                RoleId = RoleQaId,
                Active = true,
                CreatedDate = now,
                UserUpdated = SystemUser
            },
            new ProjectMember
            {
                Id = Guid.Parse("ad5f5a2a-2f07-4c56-a0b8-5b8c1c476d51"),
                ProjectId = ProjectGammaId,
                UserId = UserAliceId,
                RoleId = RoleDeveloperId,
                Active = false,
                CreatedDate = now.AddDays(-10),
                UserUpdated = SystemUser
            }
        ];

        public static IReadOnlyList<TaskTemplate> TaskTemplates =>
        [
            new TaskTemplate("Kickoff meeting", "Schedule project kickoff with stakeholders.", 7),
            new TaskTemplate("Backlog grooming", "Refine and prioritize the initial backlog.", 14),
            new TaskTemplate("First delivery", "Ship the first milestone to staging.", 30),
            new TaskTemplate("Post-launch review", "Collect feedback and finalize the release notes.", 45)
        ];

        public static IReadOnlyList<TaskItem> BuildSeedTasks(DateTime now)
        {
            var templates = TaskTemplates;

            return
            [
                new TaskItem
                {
                    Id = Guid.Parse("6608302c-9b26-4a8e-aadb-88e824a6ca5c"),
                    Title = templates[0].Title,
                    Description = templates[0].Description,
                    ProjectId = ProjectAlphaId,
                    AssignedUserId = UserAliceId,
                    Status = Domain.Enums.TaskState.New,
                    CreatedAt = now.AddDays(-2),
                    CreatedDate = now.AddDays(-2),
                    DueDate = now.AddDays(templates[0].DueInDays),
                    UserUpdated = SystemUser
                },
                new TaskItem
                {
                    Id = Guid.Parse("51b6a63b-9f49-4ff7-9ded-8f55d1de9d8b"),
                    Title = templates[1].Title,
                    Description = templates[1].Description,
                    ProjectId = ProjectAlphaId,
                    AssignedUserId = UserBobId,
                    Status = Domain.Enums.TaskState.InProgress,
                    CreatedAt = now.AddDays(-10),
                    CreatedDate = now.AddDays(-10),
                    DueDate = now.AddDays(templates[1].DueInDays),
                    UserUpdated = SystemUser
                },
                new TaskItem
                {
                    Id = Guid.Parse("3f771022-58b7-4f6a-9ff5-cc352c6d94a1"),
                    Title = templates[2].Title,
                    Description = templates[2].Description,
                    ProjectId = ProjectBetaId,
                    AssignedUserId = UserEmmaId,
                    Status = Domain.Enums.TaskState.New,
                    CreatedAt = now.AddDays(-20),
                    CreatedDate = now.AddDays(-20),
                    DueDate = now.AddDays(templates[2].DueInDays),
                    UserUpdated = SystemUser
                }
            ];
        }
    }

    public sealed record TaskTemplate(string Title, string Description, int DueInDays);
}
