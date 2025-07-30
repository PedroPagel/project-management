using FluentValidation;
using Project.Management.Domain.Services.Roles.Models;

namespace Project.Management.Domain.Services.Roles.Validators
{
    public class RoleCreationValidator : AbstractValidator<RoleCreateRequest>
    {
        public RoleCreationValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .MinimumLength(3)
                .WithMessage("Name is required.");
        }
    }
}
