using FluentValidation;
using Project.Management.Domain.Services.Users.Models;

namespace Project.Management.Domain.Services.Users.Validators
{
    public class UserUpdateValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateValidator()
        {
            RuleFor(user => user.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.")
                .When(user => !string.IsNullOrWhiteSpace(user.FullName));

            RuleFor(user => user.Email)
                .EmailAddress()
                .WithMessage("A valid email is required.")
                .When(user => !string.IsNullOrWhiteSpace(user.Email));

            RuleFor(user => user.PasswordHash)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
                .When(user => !string.IsNullOrWhiteSpace(user.PasswordHash));
        }
    }
}
