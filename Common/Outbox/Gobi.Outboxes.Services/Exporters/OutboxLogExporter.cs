using Gobi.Outboxes.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Gobi.Outboxes.Services.Exporters;

public sealed class OutboxLogExporter : IOutboxExporter
{
    private readonly ILogger<OutboxLogExporter> _logger;

    public OutboxLogExporter(ILogger<OutboxLogExporter> logger)
    {
        _logger = logger;
    }

    public Task ExportAsync(IEnumerable<OutboxEvent> outboxEvent, CancellationToken cancellationToken = default)
    {
        foreach (var e in outboxEvent)
            _logger.LogInformation("Export OutboxEvent - offset: {Offset}, disc: {Discriminator}, created: {Created} ",
                e.Offset, e.Discriminator, e.Created);

        return Task.CompletedTask;
    }
}