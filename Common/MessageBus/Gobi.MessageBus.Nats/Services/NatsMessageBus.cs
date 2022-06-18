using Gobi.MessageBus.Nats.Options;
using Gobi.MessageBus.Services.Models;
using Gobi.MessageBus.Services.Serializers;
using Gobi.MessageBus.Services.Services;
using NATS.Client;
using NATS.Client.JetStream;

namespace Gobi.MessageBus.Nats.Services;

public sealed class NatsMessageBus : IMessageBus, IDisposable
{
    private readonly IMessageSerializer _serializer;
    private readonly IConnection _connection;
    private readonly IJetStream _jetStream;

    public NatsMessageBus(NatsMessageBusOptions options, IMessageSerializer serializer)
    {
        _serializer = serializer;
        var connectionOptions = ConnectionFactory.GetDefaultOptions();
        connectionOptions.Url = options.Url;
        
        _connection = new ConnectionFactory().CreateConnection(connectionOptions);
        _jetStream = _connection.CreateJetStreamContext();
    }

    public async Task PublishAsync<T>(string subject, Message<T> message, CancellationToken cancellationToken = default)
        where T : class
    {
        using var msBody = new MemoryStream();
        var discriminator = _serializer.Serialize(msBody, message.Body);
        
        //todo: optimize
        using var msRecord = new MemoryStream();
        MessageRecordSerializer.Serialize(msRecord, discriminator, msBody.GetBuffer()[..(int)msBody.Length],
            message.CorrelationId, message.Timestamp);
        
        await _jetStream.PublishAsync(subject, msRecord.ToArray());
    }

    public IMessageBusConsumer<T> ConsumeAsync<T>(string subject, CancellationToken cancellationToken = default)
        where T : class
    {
        return new NatsMessageBusConsumer<T>(_jetStream, _serializer);
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}