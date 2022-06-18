using System;
using System.Threading.Tasks;
using FluentAssertions;
using Gobi.MessageBus.Json;
using Gobi.MessageBus.Nats.Options;
using Gobi.MessageBus.Nats.Services;
using Gobi.MessageBus.Services.Models;
using Xunit;

namespace Gobi.MessageBus.Tests.Services;

[Collection("nats")]
public sealed class NatsMessageBusTests
{
    private readonly TestNatsFixture _fixture;

    public NatsMessageBusTests(TestNatsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Consume_DurableQueue_Consumed()
    {
        // arrange
        var stream = "test_stream";
        var subject = "test_subject";
        var consumerName = "consumer";
        var queueGroup = "group";

        _fixture.ReCreateStream(stream, subject);
        _fixture.CreateConsumer(stream, consumerName, queueGroup);

        var messageBus = new NatsMessageBus(new NatsMessageBusOptions
        {
            Url = TestNatsFixture.Url
        }, new JsonMessageSerializer(typeof(NatsMessageBusTests).Assembly));

        var message = new Message<DummyMessage>(new DummyMessage
            {
                Content = "content"
            }, Guid.NewGuid(),
            DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc));
        await messageBus.PublishAsync(subject, message);

        // act
        var consumer = messageBus.ConsumeAsync<BaseMessage>(subject);
        var tcs = new TaskCompletionSource<Message<BaseMessage>>();
        consumer.Start(stream, subject, consumerName, queueGroup, msg => tcs.SetResult(msg));

        var result = await tcs.Task;

        //assert
        result.Should().BeEquivalentTo(message);
    }

    private abstract class BaseMessage
    {
    }

    private class DummyMessage : BaseMessage
    {
        public string Content { get; set; }
    }
}