namespace Gobi.Outboxes.Services.Services;

public interface IOutboxPublisher
{
    Task PublishAsync<T>(IEnumerable<T> events) where T : class;
    Task PublishAsync<T>(T @event) where T : class;
}