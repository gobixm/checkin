using Xunit;

namespace AccountTests;

[CollectionDefinition("db")]
public sealed class TestDbCollection : ICollectionFixture<TestDbFixture>
{
    
}