using Microsoft.Extensions.Logging;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;
using System.Linq.Expressions;

namespace Project.Management.Domain.Services.Projects
{
    public class ProjectMemberService(INotificator notificator, IProjectMemberRepository memberRepository, ILogger<ProjectMemberService> logger)
        : BaseRepositoryService<ProjectMember>(notificator, logger, memberRepository), IProjectMemberService
    {
        private readonly IProjectMemberRepository _memberRepository = memberRepository;

        public async Task<ProjectMember> Create(ProjectMember member) => await _memberRepository.Create(member);

        public async Task<ProjectMember> Update(ProjectMember member) => await _memberRepository.Update(member);

        public async Task<bool> Delete(Guid userId, Guid projectId) => await _memberRepository.Delete(userId, projectId);

        public async Task<ProjectMember> GetById(Guid userId, Guid projectId)
        {
            return await _memberRepository.FirstOrDefault(pm => pm.UserId == userId && pm.Id == projectId);
        }

        public Task<ProjectMember> GetProject(Expression<Func<ProjectMember, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectMember>> GetProjects(Expression<Func<ProjectMember, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
