namespace Checkin.Common.Repositories.Repositories;

public sealed class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IServiceProvider _serviceProvider;

    public UnitOfWorkFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IUnitOfWork Create()
    {
        return new UnitOfWork(_serviceProvider);
    }

    public async Task ExecuteAsync(Func<IUnitOfWork, CancellationToken, Task> action,
        CancellationToken cancellationToken = default)
    {
        await using var unitOfWork = Create();

        await action(unitOfWork, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task ExecuteAsync(Action<IUnitOfWork, CancellationToken> action,
        CancellationToken cancellationToken = default)
    {
        await using var unitOfWork = Create();

        action(unitOfWork, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);
    }

    public async Task<T> QueryAsync<T>(Func<IUnitOfWork, CancellationToken, Task<T>> action,
        CancellationToken cancellationToken = default)
    {
        await using var unitOfWork = Create();

        var result = await action(unitOfWork, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);
        return result;
    }
}