using FluentValidation;
using Project.Management.Domain.Services.Tasks.Models;

namespace Project.Management.Domain.Services.Tasks.Validations
{
    public class TaskItemUpdateValidation : AbstractValidator<TaskItemUpdateRequest>
    {
        public TaskItemUpdateValidation(Enums.TaskStatus currentStatus)
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
             .NotEqual(Enums.TaskStatus.New)
             .When(taskItem => currentStatus != Enums.TaskStatus.InProgress)
             .WithMessage("Status must be informed as In.");
        }
    }
}
