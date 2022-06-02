using Checkin.AccountService.Service.Models;

namespace Checkin.AccountService.Service.Services;

public interface IAccountService
{
    Task<CreateAccountResponse> CreateAccountAsync(CreateAccountRequest createRequest,
        CancellationToken cancellationToken = default);

    Task DeleteAccountAsync(int accountId, CancellationToken cancellationToken = default);
    Task<bool> IsLoginExistsAsync(string login, CancellationToken cancellationToken = default);
}