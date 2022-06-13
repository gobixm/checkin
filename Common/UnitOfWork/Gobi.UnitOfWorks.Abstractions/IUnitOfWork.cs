namespace Gobi.UnitOfWorks.Abstractions;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    T GetRepository<T>() where T : notnull;
    Task SaveAsync(CancellationToken cancellationToken = default);
}