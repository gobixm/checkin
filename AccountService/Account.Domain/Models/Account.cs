namespace AccountDomain.Models;

public sealed class Account
{
    public int AccountId { get; set; }
    public string Name { get; set; } = null!;
    public List<string>? Interests { get; set; }
    public DateTime Created { get; set; }
    public List<AccountFriend> Friends { get; set; } = null!;
}