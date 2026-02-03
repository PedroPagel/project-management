using Project.Management.Domain.Services.Users;
using Project.Management.Domain.Services.Users.Models;

namespace Project.Management.Api.Messaging
{
    public class InMemoryUserCreationListener(
        ILogger<InMemoryUserCreationListener> logger,
        IServiceScopeFactory scopeFactory,
        IUserCreationQueue queue) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("In-memory user creation listener started.");

            await foreach (var message in queue.Reader.ReadAllAsync(stoppingToken))
            {
                using var scope = scopeFactory.CreateScope();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                var request = new UserCreationRequest
                {
                    FullName = message.FullName,
                    Email = message.Email,
                    PasswordHash = message.PasswordHash
                };

                var result = await userService.Create(request);

                if (result == null)
                {
                    logger.LogWarning("User creation failed for email {Email} (in-memory).", message.Email);
                }
                else
                {
                    logger.LogInformation("User created from in-memory message for email {Email}.", message.Email);
                }
            }
        }
    }
}
