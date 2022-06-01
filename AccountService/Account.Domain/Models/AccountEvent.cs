using System.Text.Json;

namespace AccountDomain.Models;

public sealed class AccountEvent
{
    public int AccountEventId { get; set; }
    public AccountEventDiscriminator Discriminator { get; set; }
    public JsonDocument Body { get; set; } = null!;
    public DateTime Created { get; set; }
}