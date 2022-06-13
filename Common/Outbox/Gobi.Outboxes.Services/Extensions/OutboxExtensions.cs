using Gobi.Outboxes.Services.Exporters;
using Gobi.Outboxes.Services.Options;
using Gobi.Outboxes.Services.Serializers;
using Gobi.Outboxes.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gobi.Outboxes.Services.Extensions;

public static class OutboxExtensions
{
    public static IServiceCollection AddOutbox(this IServiceCollection serviceCollection,
        Action<OutboxConsumeServiceOptions>? configure = null)
    {
        var options = new OutboxConsumeServiceOptions();
        configure?.Invoke(options);

        serviceCollection.AddHostedService(sp => new OutboxConsumeService(options, sp,
            sp.GetRequiredService<IOutboxExporter>(),
            sp.GetRequiredService<ILogger<OutboxConsumeService>>()));

        serviceCollection.AddTransient<IOutboxPublisher, OutboxPublisher>();
        serviceCollection.AddTransient<IOutboxSerializer, OutboxSerializer>();

        return serviceCollection;
    }
}