namespace Checkin.Common.Repositories.Repositories;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    T GetRepository<T>() where T : notnull;
    Task SaveAsync(CancellationToken cancellationToken = default);
}