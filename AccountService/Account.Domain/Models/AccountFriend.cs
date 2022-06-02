namespace Checkin.AccountService.Domain.Models;

public sealed class AccountFriend
{
    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;
    
    public int FriendId { get; set; }
    public Account Friend { get; set; } = null!;
    
    public DateTime Created { get; set; }
}