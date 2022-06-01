using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountDomain.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AccountTests.Domains;

[Collection("db")]
public sealed class AccountTests
{
    private readonly TestDbFixture _fixture;

    public AccountTests(TestDbFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Save_Account_Saved()
    {
        // arrange
        await using var writeContext = _fixture.CreateContext();

        var account = new Account
        {
            Name = "foo",
            Interests = new List<string> { "tag1", "tag2" }
        };

        // act
        await writeContext.Accounts.AddAsync(account);
        await writeContext.SaveChangesAsync();

        //assert
        await using var readContext = _fixture.CreateContext();
        var savedAccount = readContext.Accounts
            .FirstOrDefault(x => x.AccountId == account.AccountId);

        savedAccount.Should().BeEquivalentTo(account);
    }

    [Fact]
    public async Task SaveFriends_Friends_Saved()
    {
        // arrange
        await using var writeContext = _fixture.CreateContext();
        var account = new Account { Name = "user" };
        var friend1 = new Account { Name = "friend1" };
        var friend2 = new Account { Name = "friend2" };

        // act
        account.Friends = new List<AccountFriend>
        {
            new() { Account = account, Friend = friend1 },
            new() { Account = account, Friend = friend2 }
        };
        await writeContext.Accounts.AddRangeAsync(account);

        await writeContext.SaveChangesAsync();

        //assert
        await using var readContext = _fixture.CreateContext();
        var savedAccount = readContext.Accounts
            .Include(x => x.Friends)
            .ThenInclude(x => x.Friend)
            .FirstOrDefault(x => x.AccountId == account.AccountId);

        savedAccount.Friends.Should().HaveCount(2);
        savedAccount.Friends[0].Friend.Name.Should().Be(account.Friends[0].Friend.Name);
        savedAccount.Friends[1].Friend.Name.Should().Be(account.Friends[1].Friend.Name);
    }
}