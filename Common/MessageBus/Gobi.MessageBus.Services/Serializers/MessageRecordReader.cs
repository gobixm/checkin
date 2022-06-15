using Gobi.Encodings.Encoders.Numbers;

namespace Gobi.MessageBus.Services.Serializers;

public sealed class MessageRecordReader
{
    public MessageRecordReader(ReadOnlyMemory<byte> data)
    {
        var offset = 0;

        // discriminator
        var memory = data;
        var (value, size) = VariableLengthEncoder.DecodeUint(memory.Span);
        memory = memory[size..];
        Discriminator = memory[..(int)value];
        memory = memory[(int)value..];

        // body
        (value, size) = VariableLengthEncoder.DecodeUint(memory.Span);
        memory = memory[size..];
        Body = memory[..(int)value];
        memory = memory[(int)value..];

        while (memory.Length > 0)
        {
            (value, size) = VariableLengthEncoder.DecodeUint(memory.Span);
            var field = (MessageRecordFields)value;
            memory = memory[size..];

            switch (field)
            {
                case MessageRecordFields.CorrelationId:
                    CorrelationId = new Guid(memory.Span[..16]);
                    memory = memory[16..];
                    break;

                case MessageRecordFields.Timestamp:
                    var (epoch, s) = VariableLengthEncoder.DecodeULong(memory.Span);
                    Timestamp = DateTime.UnixEpoch.AddMilliseconds(epoch);
                    memory = memory[s..];
                    break;
            }
        }
    }

    public ReadOnlyMemory<byte> Discriminator { get; }
    public ReadOnlyMemory<byte> Body { get; }
    public Guid? CorrelationId { get; }
    public DateTime? Timestamp { get; }
}