using FluentValidation;
using Project.Management.Domain.Services.Projects.Models;

namespace Project.Management.Domain.Services.Projects.Validators
{
    public class ProjectUpdateValidator : AbstractValidator<ProjectUpdateRequest>
    {
        public ProjectUpdateValidator(Entities.Project project)
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .When(p => !p.Name.Equals(project.Name))
                .WithMessage($"Project with the name {project.Name} already exists.");

            RuleFor(user => user.Name)
               .NotEqual(project.Name)
               .WithMessage($"Project with the name {project.Name} already exists.");

            RuleFor(user => user.Description)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Description must be at least 6 characters long.")
                .When(p => !p.Description.Equals(project.Description));

            RuleFor(user => user.StartDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Start date cannot be in the past.")
                .When(p => p.StartDate != project.StartDate);

            RuleFor(user => user.EndDate)
                .NotEmpty()
                .GreaterThan(user => user.StartDate)
                .WithMessage("End date must be after the start date.")
                .When(p => p.EndDate != project.EndDate);
        }
    }
}
