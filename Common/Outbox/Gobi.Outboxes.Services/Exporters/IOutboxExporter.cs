using Gobi.Outboxes.Domain.Models;

namespace Gobi.Outboxes.Services.Exporters;

public interface IOutboxExporter
{
    public Task ExportAsync(IEnumerable<OutboxEvent> outboxEvent, CancellationToken cancellationToken = default);
}