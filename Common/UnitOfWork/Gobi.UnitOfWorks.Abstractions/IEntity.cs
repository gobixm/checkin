namespace Gobi.UnitOfWorks.Abstractions;

public interface IEntity<TKey>
{
    public TKey Id { get; set; }
}