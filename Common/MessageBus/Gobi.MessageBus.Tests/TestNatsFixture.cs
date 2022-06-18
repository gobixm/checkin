using Gobi.MessageBus.Nats.Utils;
using NATS.Client;
using NATS.Client.JetStream;

namespace Gobi.MessageBus.Tests;

public sealed class TestNatsFixture
{
    public const string Url = "nats://localhost:4222";

    public void ReCreateStream(string stream, params string[] subjects)
    {
        var connectionOptions = ConnectionFactory.GetDefaultOptions();
        connectionOptions.Url = Url;

        using var connection = new ConnectionFactory().CreateConnection(connectionOptions);
        var jetStream = connection.CreateJetStreamManagementContext();
        if (NatsStreamUtils.StreamExists(jetStream, stream))
            NatsStreamUtils.DeleteStream(jetStream, stream);

        NatsStreamUtils.CreateStream(jetStream, stream, StorageType.Memory, subjects);
    }

    public void CreateConsumer(string stream, string consumerName, string queueGroup)
    {
        var connectionOptions = ConnectionFactory.GetDefaultOptions();
        connectionOptions.Url = Url;

        using var connection = new ConnectionFactory().CreateConnection(connectionOptions);
        var jetStream = connection.CreateJetStreamManagementContext();
        var cc = ConsumerConfiguration.Builder()
            .WithDurable(consumerName)
            .WithDeliverSubject(consumerName)
            .WithDeliverGroup(queueGroup)
            .Build();
        jetStream.AddOrUpdateConsumer(stream, cc);
    }
}