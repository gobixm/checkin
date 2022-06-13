using Gobi.UnitOfWorks.Abstractions;

namespace Checkin.AccountService.Domain.Models;

public sealed class Account : IEntity<Guid>
{
    public string Name { get; set; } = null!;
    public string Login { get; set; } = null!;
    public List<string>? Interests { get; set; }
    public DateTime Created { get; set; }
    public List<AccountFriend> Friends { get; set; } = null!;
    public Guid Id { get; set; }
}