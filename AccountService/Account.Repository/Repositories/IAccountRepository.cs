using Checkin.AccountService.Domain.Models;
using Gobi.UnitOfWorks.Abstractions;

namespace Checkin.AccountService.Repositories.Repositories;

public interface IAccountRepository : IRepository<Guid, Account>
{
    Task<bool> GetLoginExistsAsync(string login, CancellationToken cancellationToken = default);
}