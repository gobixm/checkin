using Gobi.Outboxes.Domain.Models;
using Gobi.Outboxes.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gobi.Outboxes.Ef;

public sealed class OutboxEfRepository : IOutboxRepository
{
    private readonly DbSet<OutboxEvent> _dbSet;

    public OutboxEfRepository(DbContext context)
    {
        _dbSet = context.Set<OutboxEvent>();
    }

    public async Task AddAsync(IEnumerable<OutboxEvent> events, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(events, cancellationToken);
    }

    public Task DeleteAsync(IEnumerable<OutboxEvent> events, CancellationToken cancellationToken = default)
    {
        _dbSet.RemoveRange(events);

        return Task.CompletedTask;
    }

    public Task<OutboxEvent[]> GetNextAsync(int offset, int take, CancellationToken cancellationToken = default)
    {
        return _dbSet.Where(x => x.Offset > offset)
            .Take(take)
            .OrderBy(x => x.Offset)
            .ToArrayAsync(cancellationToken);
    }
}