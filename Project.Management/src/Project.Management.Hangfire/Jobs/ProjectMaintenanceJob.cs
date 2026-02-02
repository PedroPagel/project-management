using Microsoft.EntityFrameworkCore;
using Project.Management.Domain.Enums;
using Project.Management.Infrastructure.Data;

namespace Project.Management.Hangfire.Jobs
{
    public class ProjectMaintenanceJob(ProjectManagementDbContext db, ILogger<ProjectMaintenanceJob> logger)
    {
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            await AddTasksForNewProjectsAsync(now, cancellationToken);
            await BlockExpiredTasksAsync(now, cancellationToken);
        }

        private async Task AddTasksForNewProjectsAsync(DateTime now, CancellationToken cancellationToken)
        {
            var projectsWithoutTasks = await db.Projects
                .Include(project => project.Tasks)
                .Where(project => !project.Tasks.Any())
                .ToListAsync(cancellationToken);

            if (projectsWithoutTasks.Count == 0)
            {
                return;
            }

            var users = await db.Users
                .OrderBy(user => user.FullName)
                .ToListAsync(cancellationToken);

            var templates = SeedDataFactory.TaskTemplates;
            var createdTasks = new List<Domain.Entities.TaskItem>();

            foreach (var project in projectsWithoutTasks)
            {
                var assignedUser = users.FirstOrDefault();

                foreach (var template in templates)
                {
                    createdTasks.Add(new Domain.Entities.TaskItem
                    {
                        Id = Guid.NewGuid(),
                        Title = template.Title,
                        Description = template.Description,
                        ProjectId = project.Id,
                        AssignedUserId = assignedUser?.Id,
                        Status = TaskState.New,
                        CreatedAt = now,
                        CreatedDate = now,
                        DueDate = now.AddDays(template.DueInDays),
                        UserUpdated = SeedDataFactory.SystemUser
                    });
                }
            }

            db.Tasks.AddRange(createdTasks);
            await db.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Created {TaskCount} tasks for {ProjectCount} new projects.", createdTasks.Count, projectsWithoutTasks.Count);
        }

        private async Task BlockExpiredTasksAsync(DateTime now, CancellationToken cancellationToken)
        {
            var cutoffDate = now.AddDays(-14);

            var expiredTasks = await db.Tasks
                .Where(task => task.Status != TaskState.Blocked && task.CreatedAt <= cutoffDate)
                .ToListAsync(cancellationToken);

            if (expiredTasks.Count == 0)
            {
                return;
            }

            foreach (var task in expiredTasks)
            {
                task.Status = TaskState.Blocked;
                task.UpdatedDate = now;
                task.UserUpdated = SeedDataFactory.SystemUser;
            }

            await db.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Blocked {TaskCount} tasks older than 14 days.", expiredTasks.Count);
        }
    }
}
