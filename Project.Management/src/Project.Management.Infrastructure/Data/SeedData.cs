using Project.Management.Domain.Entities;

namespace Project.Management.Infrastructure.Data
{
    public partial class ProjectManagementDbContext
    {
        public void SeedData()
        {
            if (ProjectMembers.Any())
            {
                return;
            }

            var now = DateTime.UtcNow;

            var users = SeedDataFactory.BuildUsers(now);
            Users.AddRange(users);

            var roles = SeedDataFactory.BuildRoles(now);
            Roles.AddRange(roles);

            var projects = SeedDataFactory.BuildProjects(now);
            Projects.AddRange(projects);

            var members = SeedDataFactory.BuildProjectMembers(now);
            ProjectMembers.AddRange(members);

            var tasks = SeedDataFactory.BuildSeedTasks(now);
            Tasks.AddRange(tasks);

            SaveChanges();
        }
    }
}
