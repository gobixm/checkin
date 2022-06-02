using Checkin.AccountService.Domain.Models;
using Checkin.Common.Repositories.Repositories;

namespace Checkin.AccountService.Repository.Repositories;

public interface IAccountRepository : IRepository<int, Account>
{
    Task<bool> GetLoginExistsAsync(string login, CancellationToken cancellationToken = default);
}