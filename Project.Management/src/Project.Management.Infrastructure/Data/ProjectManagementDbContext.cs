using Microsoft.EntityFrameworkCore;
using Project.Management.Domain.Entities;

namespace Project.Management.Infrastructure.Data
{
    public partial class ProjectManagementDbContext(DbContextOptions<ProjectManagementDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Domain.Entities.Project> Projects { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectManagementDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
