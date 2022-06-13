using Gobi.Outboxes.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Gobi.Outboxes.Ef.Extensions;

public static class OutboxEfExtensions
{
    public static IServiceCollection AddOutboxEntityFramework(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IOutboxRepository, OutboxEfRepository>();

        return serviceCollection;
    }
}