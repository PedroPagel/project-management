using Microsoft.EntityFrameworkCore;
using Project.Management.Domain.Entities;
using Project.Management.Infrastructure.Data.Mappings;

namespace Project.Management.Infrastructure.Data
{
    public class ProjectManagementDbContext(DbContextOptions<ProjectManagementDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Domain.Entities.Project> Projects { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new ProjectMapping());
            modelBuilder.ApplyConfiguration(new RoleMapping());
            modelBuilder.ApplyConfiguration(new ProjectMemberMapping());
            modelBuilder.ApplyConfiguration(new TaskItemMapping());
        }
    }
}
