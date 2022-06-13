using Gobi.UnitOfWorks.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Gobi.UnitOfWorks.Ef;

public static class UnitOfWorkExtensions
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
    }
}