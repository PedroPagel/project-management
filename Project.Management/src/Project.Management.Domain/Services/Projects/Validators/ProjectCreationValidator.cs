using FluentValidation;
using Project.Management.Domain.Services.Projects.Models;

namespace Project.Management.Domain.Services.Projects.Validators
{
    public class ProjectCreationValidator : AbstractValidator<ProjectCreationRequest>
    {
        public ProjectCreationValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .WithMessage("Name is required.");

            RuleFor(user => user.Description)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Description must be at least 6 characters long.");

            RuleFor(user => user.StartDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.UtcNow.AddMinutes(-1))
                .WithMessage("Start date cannot be in the past.");

            RuleFor(user => user.EndDate)
                .NotEmpty()
                .GreaterThan(user => user.StartDate)
                .WithMessage("End date must be after the start date.");
        }
    }
}
