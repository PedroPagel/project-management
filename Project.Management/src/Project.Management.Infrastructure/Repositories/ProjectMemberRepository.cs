using Microsoft.EntityFrameworkCore;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Projects.Models;
using Project.Management.Infrastructure.Data;

namespace Project.Management.Infrastructure.Repositories
{
    public class ProjectMemberRepository(ProjectManagementDbContext db) : Repository<ProjectMember>(db), IProjectMemberRepository
    {
        public async Task<bool> Delete(Guid userId, Guid id)
        {
            var projectMember = await DbSet.FirstOrDefaultAsync(pm => pm.Id == id && pm.UserId == userId);

            DbSet.Remove(projectMember);
            return await SaveChanges();
        }

        public async Task<ProjectMemberQueryValidation> GetProjectMemberLink(ProjectMemberCreationRequest projectmemberCreationRequest)
        {
            return await (from projectMember in Db.ProjectMembers.Where(pm => pm.ProjectId == projectmemberCreationRequest.ProjectId &&
                                                                              pm.UserId == projectmemberCreationRequest.UserId &&
                                                                              pm.RoleId == projectmemberCreationRequest.RoleId).DefaultIfEmpty()
                                select new ProjectMemberQueryValidation()
                                {
                                    ProjectMemberId = projectMember == null ? Guid.Empty : projectMember.Id,
                                    ProjectExists = Db.Projects.Any(p => p.Id == projectmemberCreationRequest.ProjectId),
                                    UserExists = Db.Users.Any(u => u.Id == projectmemberCreationRequest.UserId),
                                    RoleExists = Db.Roles.Any(r => r.Id == projectmemberCreationRequest.RoleId)
                                }).FirstOrDefaultAsync();
        }
    }
}
