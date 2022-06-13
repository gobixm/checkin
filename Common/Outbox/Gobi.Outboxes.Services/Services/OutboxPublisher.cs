using Gobi.Outboxes.Domain.Models;
using Gobi.Outboxes.Repositories;
using Gobi.Outboxes.Services.Serializers;
using Gobi.UnitOfWorks.Abstractions;

namespace Gobi.Outboxes.Services.Services;

public sealed class OutboxPublisher : IOutboxPublisher
{
    private readonly IOutboxRepository _repository;
    private readonly IOutboxSerializer _serializer;

    public OutboxPublisher(IOutboxSerializer serializer, IUnitOfWork unitOfWork)
    {
        _serializer = serializer;
        _repository = unitOfWork.GetRepository<IOutboxRepository>();
    }

    public async Task PublishAsync<T>(T @event) where T : class
    {
        await PublishAsync<T>(new[] { @event });
    }

    public async Task PublishAsync<T>(IEnumerable<T> events) where T : class
    {
        var entities = events.Select(x => _serializer.Serialize(x));

        await _repository.AddAsync(entities);
    }
}