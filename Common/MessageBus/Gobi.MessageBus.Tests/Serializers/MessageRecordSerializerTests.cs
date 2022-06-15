using System;
using System.IO;
using FluentAssertions;
using Gobi.MessageBus.Services.Serializers;
using Xunit;

namespace Gobi.MessageBus.Tests.Serializers;

public sealed class MessageRecordSerializerTests
{
    [Fact]
    public void Foo()
    {
        // arrange
        using var stream = new MemoryStream();
        byte[] discriminator = { 1, 2, 3 };
        byte[] body = { 4, 5, 6 };
        var correlationId = Guid.NewGuid();
        var timestamp = DateTime.Now;
        var epoch = ((DateTimeOffset)timestamp).ToUnixTimeMilliseconds();

        // act
        MessageRecordSerializer.Serialize(stream, discriminator, body, correlationId, timestamp);
        var reader = MessageRecordSerializer.Deserialize(stream.ToArray());

        //assert
        reader.Body.ToArray().Should().BeEquivalentTo(body);
        reader.Discriminator.ToArray().Should().BeEquivalentTo(discriminator);
        reader.CorrelationId.Should().Be(correlationId);
        reader.Timestamp.Should().NotBeNull();
        ((DateTimeOffset)reader.Timestamp).ToUnixTimeMilliseconds().Should().Be(epoch);
    }
}