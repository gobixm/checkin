using Gobi.Outboxes.Repositories;
using Gobi.Outboxes.Services.Exporters;
using Gobi.Outboxes.Services.Options;
using Gobi.UnitOfWorks.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gobi.Outboxes.Services.Services;

public sealed class OutboxConsumeService : BackgroundService
{
    private readonly ILogger<OutboxConsumeService> _logger;
    private readonly OutboxConsumeServiceOptions _options;
    private readonly IOutboxExporter _outboxExporter;
    private readonly IServiceProvider _serviceProvider;

    public OutboxConsumeService(
        OutboxConsumeServiceOptions options,
        IServiceProvider serviceProvider,
        IOutboxExporter outboxExporter,
        ILogger<OutboxConsumeService> logger)
    {
        _options = options;
        _serviceProvider = serviceProvider;
        _outboxExporter = outboxExporter;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var offset = 0;
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                await using var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var repository = unitOfWork.GetRepository<IOutboxRepository>();
                
                var events = await repository.GetNextAsync(offset, _options.MaxBatchSize, stoppingToken);
                if (events.Any())
                {
                    await _outboxExporter.ExportAsync(events, CancellationToken.None);
                    await repository.DeleteAsync(events, CancellationToken.None);

                    await unitOfWork.SaveAsync(CancellationToken.None);

                    offset = events[^1].Offset;
                }
                else
                {
                    //todo: track changes on notify: https://www.graymatterdeveloper.com/2019/12/02/listening-events-postgresql/
                    await Task.Delay(_options.BatchInterval, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process outbox, will retry...");
            }
        }
    }
}