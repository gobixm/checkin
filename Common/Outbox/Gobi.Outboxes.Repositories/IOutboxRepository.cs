using Gobi.Outboxes.Domain.Models;

namespace Gobi.Outboxes.Repositories;

public interface IOutboxRepository
{
    public Task AddAsync(IEnumerable<OutboxEvent> events, CancellationToken cancellationToken = default);
    public Task DeleteAsync(IEnumerable<OutboxEvent> events, CancellationToken cancellationToken = default);
    public Task<OutboxEvent[]> GetNextAsync(int offset, int take, CancellationToken cancellationToken = default);
}