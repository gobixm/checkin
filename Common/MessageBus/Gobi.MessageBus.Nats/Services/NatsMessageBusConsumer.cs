using Gobi.MessageBus.Services.Models;
using Gobi.MessageBus.Services.Serializers;
using Gobi.MessageBus.Services.Services;
using NATS.Client.JetStream;

namespace Gobi.MessageBus.Nats.Services;

public sealed class NatsMessageBusConsumer<T> : IMessageBusConsumer<T> where T : class
{
    private readonly IJetStream _jetStream;
    private readonly IMessageSerializer _serializer;

    public NatsMessageBusConsumer(IJetStream jetStream, IMessageSerializer serializer)
    {
        _jetStream = jetStream;
        _serializer = serializer;
    }

    public IDisposable Start(
        string stream,
        string subject,
        string consumerName,
        string queueGroup,
        Action<Message<T>> processor)
    {
        var options = PushSubscribeOptions.BindTo(stream, consumerName);
        var sub = _jetStream.PushSubscribeSync(subject, queueGroup, options);
        var cts = new CancellationTokenSource();

        new Thread(() =>
        {
            while (!cts.IsCancellationRequested)
            {
                var message = sub.NextMessage(500);

                var recordReader = MessageRecordSerializer.Deserialize(message.Data);
                var body = _serializer.Deserialize<T>(recordReader.Body.Span, recordReader.Discriminator.Span);

                if (body is null)
                    throw new ArgumentException("Failed to deserialize message");
                processor(new Message<T>(body, recordReader.CorrelationId, recordReader.Timestamp));

                message.Ack();
            }
        }).Start();

        return new DelegateDisposable(() => cts.Cancel());
    }

    private class DelegateDisposable : IDisposable
    {
        private readonly Action _dispose;

        public DelegateDisposable(Action dispose)
        {
            _dispose = dispose;
        }

        public void Dispose()
        {
            _dispose();
        }
    }
}