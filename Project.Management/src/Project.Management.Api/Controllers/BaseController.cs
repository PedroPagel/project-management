using Microsoft.AspNetCore.Mvc;
using Project.Management.Domain.Services.Notificator;

namespace Project.Management.Api.Controllers
{
    public class BaseController(INotificator notificator) : ControllerBase
    {
        private readonly INotificator _notificator = notificator;

        [NonAction]
        public async Task<ActionResult> CustomResponse(object result)
        {
            if (!_notificator.GetErrorNotifications().Any())
            {
                return await Task.FromResult(new OkObjectResult(new
                {
                    success = true,
                    data = result,
                }));
            }

            var notification = _notificator.GetErrorNotifications().FirstOrDefault(n => n.StatusCode == StatusCodes.Status404NotFound ||
                n.StatusCode == StatusCodes.Status400BadRequest);

            if (notification is not null)
            {
                return notification.StatusCode switch
                {
                    400 => new BadRequestObjectResult(new
                    {
                        success = false,
                        message = notification.Message,
                    }),
                    404 => new NotFoundObjectResult(new
                    {
                        success = false,
                        message = notification.Message,
                    }),
                    409 => new ConflictObjectResult(new
                    {
                        success = false,
                        message = notification.Message,
                    }),

                    _ => throw new NotImplementedException()
                };
            }

            return await Task.FromResult(new ObjectResult(new
            {
                success = false,
                message = _notificator.GetErrorNotifications().Select(n => n.Message).ToList()
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            });
        }
    }
}

