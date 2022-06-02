using System.Linq.Expressions;
using Checkin.Common.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Checkin.Common.Repositories.Repositories;

public class Repository<TKey, TEntity> : IRepository<TKey, TEntity> where TEntity : class, IEntity<TKey>, new()
{
    public Repository(DbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    protected DbContext Context { get; }
    protected DbSet<TEntity> DbSet { get; }

    public async ValueTask<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return (await DbSet.AddAsync(entity, cancellationToken)).Entity;
    }

    public async Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await DbSet.FindAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.ToListAsync(cancellationToken);
    }

    public void Remove(TEntity item)
    {
        DbSet.Remove(item);
    }

    public void Remove(TKey id)
    {
        var entity = new TEntity { Id = id };
        DbSet.Attach(entity);
        DbSet.Remove(entity);
    }

    public TEntity UpdateAsync(TEntity item)
    {
        return DbSet.Update(item).Entity;
    }

    public async Task<IReadOnlyCollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(predicate).ToListAsync(cancellationToken);
    }
}