using Checkin.AccountService.Domain.Models;
using Checkin.Common.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Checkin.AccountService.Repository.Repositories;

public sealed class AccountRepository : Repository<int, Account>, IAccountRepository
{
    public AccountRepository(AccountDbContext context) : base(context)
    {
    }

    public async Task<bool> GetLoginExistsAsync(string login, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking().AnyAsync(x => x.Login == login, cancellationToken);
    }
}