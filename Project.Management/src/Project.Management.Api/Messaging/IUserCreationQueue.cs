using System.Threading.Channels;

namespace Project.Management.Api.Messaging
{
    public interface IUserCreationQueue
    {
        ChannelReader<UserCreationMessage> Reader { get; }
        ValueTask EnqueueAsync(UserCreationMessage message, CancellationToken cancellationToken);
    }
}
