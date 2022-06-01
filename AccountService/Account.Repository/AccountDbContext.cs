using AccountDomain.EntityMappers;
using AccountDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountRepository;

public sealed class AccountDbContext : DbContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<AccountFriend> AccountFriends => Set<AccountFriend>();
    public DbSet<AccountEvent> AccountEvents => Set<AccountEvent>();

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
        modelBuilder.ApplyConfiguration(new AccountEventMap());  
  
        base.OnModelCreating(modelBuilder);  
    }  
}