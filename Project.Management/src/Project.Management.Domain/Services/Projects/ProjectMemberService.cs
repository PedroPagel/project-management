using Microsoft.Extensions.Logging;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Extensions;
using Project.Management.Domain.Repositories;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Projects.Models;
using Project.Management.Domain.Services.Projects.Validators;

namespace Project.Management.Domain.Services.Projects
{
    public class ProjectMemberService(INotificator notificator, 
                                      IProjectMemberRepository memberRepository, 
                                      ILogger<ProjectMemberService> logger)
        : BaseRepositoryService<ProjectMember>(notificator, logger, memberRepository), IProjectMemberService
    {
        private readonly IProjectMemberRepository _memberRepository = memberRepository;

        public async Task<ProjectMember> Create(ProjectMemberCreationRequest request)
        {
            if (Validate(new ProjectMemberCreationValidation(), request))
            {
                var projectMemberValidation = await _memberRepository.GetProjectMemberLink(request);

                if (!projectMemberValidation.UserExists)
                {
                    NotifyErrorNotFound($"User not found, with the id: {request.UserId}");
                    return null;
                }

                if (!projectMemberValidation.RoleExists)
                {
                    NotifyErrorNotFound($"Role not found, with the id: {request.RoleId}");
                    return null;
                }

                if (!projectMemberValidation.ProjectExists)
                {
                    NotifyErrorNotFound($"Project not found, with the id: {request.ProjectId}");
                    return null;
                }

                if (projectMemberValidation.ProjectMemberId != Guid.Empty)
                {
                    NotifyErrorConflict($"Project member already exists, with the id: {projectMemberValidation.ProjectMemberId}");
                    return null;
                }

                return await _memberRepository.Create(new()
                {
                    RoleId = request.RoleId,
                    ProjectId = request.ProjectId,
                    UserId = request.UserId
                });
            }

            _logger.LogInformation("Project member creation failed due to validation errors");
            return null;
        }

        public async Task<ProjectMember> Update(ProjectMemberUpdateRequest request)
        {
            var projectMember = await _memberRepository.GetById(request.ProjectMemberId);

            if (projectMember is null)
            {
                NotifyErrorConflict($"Project member not found, with the id: {request.ProjectMemberId}");
                return null;
            }

            if (request.RoleId != Guid.Empty)
            {
                projectMember.UpdateIfDifferent(pm => pm.RoleId, request.RoleId);
            }

            if (request.RoleId != Guid.Empty)
            {
                projectMember.UpdateIfDifferent(pm => pm.UserId, request.UserId);
            }

            projectMember.UpdateIfDifferent(pm => pm.Active, request.Active);

            return await _memberRepository.Update(projectMember);
        }

        public async Task<IEnumerable<ProjectMember>> GetProjectMember(ProjectMemberRequest projectMemberRequest)
        {
            var projectMembers = await _memberRepository.Get(pm => ((projectMemberRequest.UserId == Guid.Empty) || (pm.UserId == projectMemberRequest.UserId)) &&
                                                                   ((projectMemberRequest.RoleId == Guid.Empty) || (pm.RoleId == projectMemberRequest.RoleId)) &&
                                                                   ((projectMemberRequest.ProjectId == Guid.Empty) || (pm.ProjectId == projectMemberRequest.ProjectId)) &&
                                                                   ((projectMemberRequest.ProjectMemberId == Guid.Empty) || (pm.Id == projectMemberRequest.ProjectMemberId)));

            return projectMembers;            
        }
    }
}
