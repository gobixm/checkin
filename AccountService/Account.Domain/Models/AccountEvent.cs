using System.Text.Json;
using Checkin.Common.Domain;

namespace Checkin.AccountService.Domain.Models;

public sealed class AccountEvent : IEntity<int>
{
    public int Id { get; set; }
    public AccountEventDiscriminator Discriminator { get; set; }
    public JsonDocument Body { get; set; } = null!;
    public DateTime Created { get; set; }
}