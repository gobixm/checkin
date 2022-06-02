using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Checkin.AccountService.Domain.Models;
using FluentAssertions;
using Xunit;

namespace Checkin.AccountService.Tests.Domains;

[Collection("db")]
public sealed class AccountEventTests
{
    private readonly TestDbFixture _fixture;

    public AccountEventTests(TestDbFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Save_AccountEvent_Saved()
    {
        // arrange
        await using var writeContext = _fixture.CreateContext();

        const string expectedJson = @"{""foo"":""bar""}";
        var domainEvent = new AccountEvent
        {
            Body = JsonDocument.Parse(expectedJson),
            Discriminator = AccountEventDiscriminator.NullEvent
        };

        // act
        await writeContext.AccountEvents.AddAsync(domainEvent);
        await writeContext.SaveChangesAsync();

        //assert
        await using var readContext = _fixture.CreateContext();
        var savedEvent = readContext.AccountEvents
            .FirstOrDefault(x => x.Id == domainEvent.Id);

        savedEvent.Should().NotBeNull();

        using var stream = new MemoryStream();
        await using var writer = new Utf8JsonWriter(stream);
        savedEvent?.Body.WriteTo(writer);
        await writer.FlushAsync();

        savedEvent?.Id.Should().NotBe(0);
        var jsonString = Encoding.UTF8.GetString(stream.ToArray());
        jsonString.Should().BeEquivalentTo(expectedJson);
    }
}