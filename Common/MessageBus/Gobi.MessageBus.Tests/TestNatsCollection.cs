using Xunit;

namespace Gobi.MessageBus.Tests;

[CollectionDefinition("nats")]
public sealed class TestNatsCollection : ICollectionFixture<TestNatsFixture>
{
    
}