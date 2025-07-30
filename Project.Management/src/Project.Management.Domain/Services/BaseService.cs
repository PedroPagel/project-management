using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Project.Management.Domain.Services.Notificator;
using System.Diagnostics.CodeAnalysis;

namespace Project.Management.Domain.Services
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseService(INotificator notificator, ILogger<BaseService> logger)
    {
        private readonly INotificator _notificator = notificator;
        internal readonly ILogger<BaseService> _logger = logger;

        private void StatusCodeErrorNotify(string message, int statusCode)
        {
            _logger.LogError("An error occurred: {Message}", message);
            _notificator.HandleError(new Notification(message, statusCode));
        }

        protected void NotifyError(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
                NotifyError(error.ErrorMessage);
        }

        protected void NotifyError(string message)
        {
            StatusCodeErrorNotify(message, StatusCodes.Status500InternalServerError);
        }

        protected void NotifyErrorBadRequest(string message)
        {
            StatusCodeErrorNotify(message, StatusCodes.Status400BadRequest);
        }

        protected void NotifyErrorNotFound(string message)
        {
            StatusCodeErrorNotify(message, StatusCodes.Status404NotFound);
        }

        protected void NotifyErrorConflict(string message)
        {
            StatusCodeErrorNotify(message, StatusCodes.Status409Conflict);
        }

        protected bool Validate<TV, T>(TV validation, T entry) where TV : AbstractValidator<T>
        {
            var validator = validation.Validate(entry);

            if (validator.IsValid)
                return true;

            NotifyError(validator);

            return false;
        }
    }
}
