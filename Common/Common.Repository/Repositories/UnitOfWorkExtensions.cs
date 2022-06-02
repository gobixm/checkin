using Microsoft.Extensions.DependencyInjection;

namespace Checkin.Common.Repositories.Repositories;

public static class UnitOfWorkExtensions
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
    }
}