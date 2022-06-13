namespace Checkin.AccountService.Contracts;

public sealed record AccountCreatedEvent(Guid Id, string[]? Interests);