namespace Checkin.AccountService.Service.Models;

public sealed record CreateAccountResponse(
    string Login,
    string Name,
    Guid Id
);