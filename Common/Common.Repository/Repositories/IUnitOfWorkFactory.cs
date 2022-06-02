namespace Checkin.Common.Repositories.Repositories;

public interface IUnitOfWorkFactory
{
    IUnitOfWork Create();

    Task ExecuteAsync(Func<IUnitOfWork, CancellationToken, Task> action,
        CancellationToken cancellationToken = default);

    Task ExecuteAsync(Action<IUnitOfWork, CancellationToken> action,
        CancellationToken cancellationToken = default);

    Task<T> QueryAsync<T>(Func<IUnitOfWork, CancellationToken, Task<T>> action,
        CancellationToken cancellationToken = default);
}