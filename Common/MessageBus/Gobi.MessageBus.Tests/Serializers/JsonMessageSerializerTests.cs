using System.IO;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Gobi.MessageBus.Json;
using Xunit;

namespace Gobi.MessageBus.Tests.Serializers;

public sealed class JsonMessageSerializerTests
{
    [Fact]
    public void Serialize_Message_Serializer()
    {
        // arrange
        var serializer = new JsonMessageSerializer(new[] { typeof(Dummy).Assembly });
        var dummy = new Dummy("foo");

        using var ms = new MemoryStream();

        // act
        var discriminator = serializer.Serialize(ms, dummy);

        //assert
        ms.Position = 0;
        var expected = serializer.Deserialize<Dummy>(ms, discriminator);

        expected.Should().BeEquivalentTo(dummy);
        Encoding.UTF8.GetString(discriminator).Should().Be(typeof(Dummy).FullName);
    }

    public class Dummy
    {
        public Dummy(string foo)
        {
            Foo = foo;
        }

        public string Foo { get; set; }
    }
}