using Checkin.AccountService.Domain.EntityMappers;
using Checkin.AccountService.Domain.Models;
using Gobi.Outboxes.Domain.Models;
using Gobi.Outboxes.Ef.EntityMappers;
using Microsoft.EntityFrameworkCore;

namespace Checkin.AccountService.Repositories;

public sealed class AccountDbContext : DbContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<AccountFriend> AccountFriends => Set<AccountFriend>();
    public DbSet<OutboxEvent> AccountEvents => Set<OutboxEvent>();

    public AccountDbContext()
    {
    }

    public AccountDbContext(DbContextOptions options) : base(options)  
    {  
    }  
  
    protected override void OnModelCreating(ModelBuilder modelBuilder)  
    {  
        modelBuilder.ApplyConfiguration(new AccountMap());  
        modelBuilder.ApplyConfiguration(new AccountFriendMap());  
        modelBuilder.ApplyConfiguration(new OutboxEventMap());    // todo: add in extenstion?  
  
        base.OnModelCreating(modelBuilder);  
    }  
}