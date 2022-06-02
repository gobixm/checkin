namespace Checkin.AccountService.Service.Models;

public sealed record CreateAccountRequest(
    string Login,
    string Name
);