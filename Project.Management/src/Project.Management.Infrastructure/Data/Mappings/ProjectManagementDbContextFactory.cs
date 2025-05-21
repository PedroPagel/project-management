using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Project.Management.Infrastructure.Data.Mappings
{
    public class ProjectManagementDbContextFactory : IDesignTimeDbContextFactory<ProjectManagementDbContext>
    {
        public ProjectManagementDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectManagementDbContext>();

            optionsBuilder.UseNpgsql("Host=localhost;Database=project_management;Username=postgres;Password=yourpassword");

            return new ProjectManagementDbContext(optionsBuilder.Options);
        }
    }
}
