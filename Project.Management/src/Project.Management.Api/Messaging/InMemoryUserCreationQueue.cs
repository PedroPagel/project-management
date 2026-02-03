using System.Threading.Channels;

namespace Project.Management.Api.Messaging
{
    public class InMemoryUserCreationQueue : IUserCreationQueue
    {
        private readonly Channel<UserCreationMessage> _channel = Channel.CreateUnbounded<UserCreationMessage>();

        public ChannelReader<UserCreationMessage> Reader => _channel.Reader;

        public ValueTask EnqueueAsync(UserCreationMessage message, CancellationToken cancellationToken)
        {
            return _channel.Writer.WriteAsync(message, cancellationToken);
        }
    }
}
