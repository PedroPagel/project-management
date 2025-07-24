using FluentValidation;
using Project.Management.Domain.Services.Users.Models;

namespace Project.Management.Domain.Services.Users.Validators
{
    public class UserCreationValidator : AbstractValidator<UserCreationRequest>
    {
        public UserCreationValidator()
        {
            RuleFor(user => user.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.");

            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email is required.");

            RuleFor(user => user.PasswordHash)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.");
        }
    }
}
