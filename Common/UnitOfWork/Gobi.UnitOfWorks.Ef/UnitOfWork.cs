using Gobi.UnitOfWorks.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Gobi.UnitOfWorks.Ef;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private readonly IServiceScope _scope;

    public UnitOfWork(IServiceProvider serviceProvider)
    {
        _scope = serviceProvider.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<DbContext>();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        _scope.Dispose();
    }

    public void Dispose()
    {
        _context.Dispose();
        _scope.Dispose();
    }

    public T GetRepository<T>() where T : notnull
    {
        return _scope.ServiceProvider.GetRequiredService<T>();
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}