using FluentValidation;
using Project.Management.Domain.Services.Tasks.Models;

namespace Project.Management.Domain.Services.Tasks.Validations
{
    public class TaskItemUpdateValidation : AbstractValidator<TaskItemUpdateRequest>
    {
        public TaskItemUpdateValidation(Enums.TaskState currentStatus)
        {
            RuleFor(taskItem => taskItem.TaskId)
                .NotEmpty()
                .WithMessage("Task id is required.");

            RuleFor(taskItem => taskItem.Status)
                .NotEmpty()
                .WithMessage("Task item title is required.");

            RuleFor(taskItem => Enum.IsDefined(taskItem.Status))
              .Equal(true)
              .WithMessage("Status must be informed.");

            RuleFor(taskItem => taskItem.Status)
             .NotEqual(Enums.TaskState.New)
             .When(taskItem => currentStatus != Enums.TaskState.InProgress)
             .WithMessage("Status must be informed as In.");
        }
    }
}
