using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Project.Management.Domain.Services.Users;
using Project.Management.Domain.Services.Users.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Project.Management.Api.Messaging
{
    public class UserCreationRabbitMqListener(
        ILogger<UserCreationRabbitMqListener> logger,
        IServiceScopeFactory scopeFactory,
        IOptions<RabbitMqOptions> options) : BackgroundService
    {
        private readonly RabbitMqOptions _options = options.Value;
        private IConnection _connection;
        private IModel _channel;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
                VirtualHost = _options.VirtualHost,
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: _options.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.BasicQos(0, 1, false);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (_, eventArgs) =>
            {
                await HandleMessageAsync(eventArgs, stoppingToken);
            };

            _channel.BasicConsume(queue: _options.QueueName, autoAck: false, consumer: consumer);

            logger.LogInformation("RabbitMQ listener started for queue {QueueName}", _options.QueueName);

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _channel?.Close();
            _connection?.Close();

            return base.StopAsync(cancellationToken);
        }

        private async Task HandleMessageAsync(BasicDeliverEventArgs eventArgs, CancellationToken cancellationToken)
        {
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            try
            {
                var payload = JsonSerializer.Deserialize<UserCreationMessage>(message, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (payload == null)
                {
                    logger.LogWarning("RabbitMQ user creation message could not be deserialized. Message: {Message}", message);
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                    return;
                }

                using var scope = scopeFactory.CreateScope();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

                var request = new UserCreationRequest
                {
                    FullName = payload.FullName,
                    Email = payload.Email,
                    PasswordHash = payload.PasswordHash
                };

                var result = await userService.Create(request);

                if (result == null)
                {
                    logger.LogWarning("User creation failed for email {Email}.", payload.Email);
                }
                else
                {
                    logger.LogInformation("User created from RabbitMQ message for email {Email}.", payload.Email);
                }

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process RabbitMQ user creation message.");
                _channel.BasicNack(eventArgs.DeliveryTag, false, false);
            }
        }
    }
}
