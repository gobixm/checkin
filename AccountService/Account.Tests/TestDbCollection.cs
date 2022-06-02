using Xunit;

namespace Checkin.AccountService.Tests;

[CollectionDefinition("db")]
public sealed class TestDbCollection : ICollectionFixture<TestDbFixture>
{
    
}