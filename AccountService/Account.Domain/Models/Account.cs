using Checkin.Common.Domain;

namespace Checkin.AccountService.Domain.Models;

public sealed class Account : IEntity<int>
{
    public string Name { get; set; } = null!;
    public string Login { get; set; } = null!;
    public List<string>? Interests { get; set; }
    public DateTime Created { get; set; }
    public List<AccountFriend> Friends { get; set; } = null!;
    public int Id { get; set; }
}