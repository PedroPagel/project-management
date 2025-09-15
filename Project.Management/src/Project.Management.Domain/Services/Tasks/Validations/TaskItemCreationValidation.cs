using FluentValidation;
using Project.Management.Domain.Services.Tasks.Models;

namespace Project.Management.Domain.Services.Tasks.Validations
{
    public class TaskItemCreationValidation : AbstractValidator<TaskItemCreationRequest>
    {
        public TaskItemCreationValidation()
        {
            RuleFor(taskItem => taskItem.ProjectId)
                .NotEmpty()
                .WithMessage("Project id is required.");

            RuleFor(taskItem => taskItem.Title)
                .NotEmpty()
                .WithMessage("Task item title is required.");

            RuleFor(taskItem => taskItem.AssignedUserId)
                .NotEmpty()
                .WithMessage("User id is required.");

            RuleFor(taskItem => taskItem.DueDate)
               .NotEmpty()
               .WithMessage("A date must be set for the task item.");
        }
    }
}
