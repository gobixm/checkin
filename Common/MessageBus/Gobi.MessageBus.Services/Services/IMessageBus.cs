using Gobi.MessageBus.Services.Models;

namespace Gobi.MessageBus.Services.Services;

public interface IMessageBus
{
    Task PublishAsync<T>(string subject, Message<T> message, CancellationToken cancellationToken = default)
        where T : class;

    IMessageBusConsumer<T> ConsumeAsync<T>(string subject, CancellationToken cancellationToken = default)
        where T : class;
}