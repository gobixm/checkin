using Checkin.AccountService.Domain.Models;
using Gobi.UnitOfWorks.Ef;
using Microsoft.EntityFrameworkCore;

namespace Checkin.AccountService.Repositories.Repositories;

public sealed class AccountRepository : Repository<Guid, Account>, IAccountRepository
{
    public AccountRepository(AccountDbContext context) : base(context)
    {
    }

    public async Task<bool> GetLoginExistsAsync(string login, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().AnyAsync(x => x.Login == login, cancellationToken);
    }
}