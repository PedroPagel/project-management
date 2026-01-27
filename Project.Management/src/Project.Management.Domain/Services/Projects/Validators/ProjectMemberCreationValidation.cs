using FluentValidation;
using Project.Management.Domain.Services.Projects.Models;

namespace Project.Management.Domain.Services.Projects.Validators
{
    public class ProjectMemberCreationValidation : AbstractValidator<ProjectMemberCreationRequest>
    {
        public ProjectMemberCreationValidation()
        {
            RuleFor(user => user.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

            RuleFor(project => project.ProjectId)
                .NotEmpty()
                .WithMessage("ProjectId is required.");

            RuleFor(role => role.RoleId)
                .NotEmpty()
                .WithMessage("RoleId is required.");
        }
    }
}
