using Checkin.AccountService.Repository;
using Microsoft.EntityFrameworkCore;

namespace Checkin.AccountService.Tests;

public sealed class TestDbFixture
{
    private const string ConnectionString =
        "Host=localhost;Port=10001;Database=account_test;Username=postgres;Password=postgres";

    public TestDbFixture()
    {
        using var context = CreateContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    public AccountDbContext CreateContext()
    {
        return new AccountDbContext(
            new DbContextOptionsBuilder<AccountDbContext>()
                .UseNpgsql(ConnectionString)
                .EnableSensitiveDataLogging()
                .Options);
    }
}