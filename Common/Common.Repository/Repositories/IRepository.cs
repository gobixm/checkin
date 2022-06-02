using System.Linq.Expressions;
using Checkin.Common.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Checkin.Common.Repositories.Repositories;

public interface IRepository<in TKey, TEntity> where TEntity : IEntity<TKey>
{
    ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    void Remove(TEntity item);
    void Remove(TKey id);
    TEntity UpdateAsync(TEntity item);
}