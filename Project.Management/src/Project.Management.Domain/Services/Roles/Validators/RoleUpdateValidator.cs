using FluentValidation;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Services.Roles.Models;

namespace Project.Management.Domain.Services.Roles.Validators
{
    public class RoleUpdateValidator : AbstractValidator<RoleUpdateRequest>
    {
        public RoleUpdateValidator(Role role)
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                .MinimumLength(3)
                .WithMessage("Name is required.");

            RuleFor(user => user.Name)
                .NotEqual(role.Name)
                .WithMessage($"Role with the name {role.Name} already exists.");
        }
    }
}
