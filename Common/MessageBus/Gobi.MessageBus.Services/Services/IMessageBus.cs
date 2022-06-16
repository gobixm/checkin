using Gobi.MessageBus.Services.Models;

namespace Gobi.MessageBus.Services.Services;

public interface IMessageBus
{
    Task PublishAsync<T>(string topic, Message<T> message, CancellationToken cancellationToken = default)
        where T : class;

    Task<IMessageConsumer> ConsumeAsync(string topic, CancellationToken cancellationToken = default);
}