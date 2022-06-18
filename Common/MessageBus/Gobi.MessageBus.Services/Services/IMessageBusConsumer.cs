using Gobi.MessageBus.Services.Models;

namespace Gobi.MessageBus.Services.Services;

public interface IMessageBusConsumer<T> where T: class
{
    IDisposable Start(
        string stream,
        string subject,
        string consumerName,
        string queueGroup, 
        Action<Message<T>> processor);
}