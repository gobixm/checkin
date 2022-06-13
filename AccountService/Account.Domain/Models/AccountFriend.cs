namespace Checkin.AccountService.Domain.Models;

public sealed class AccountFriend
{
    public Guid AccountId { get; set; }
    public Account Account { get; set; } = null!;
    
    public Guid FriendId { get; set; }
    public Account Friend { get; set; } = null!;
    
    public DateTime Created { get; set; }
}