using Gobi.Encodings.Encoders.Numbers;

namespace Gobi.MessageBus.Services.Serializers;

public static class MessageRecordSerializer
{
    public static MessageRecordReader Deserialize(ReadOnlyMemory<byte> body)
    {
        return new MessageRecordReader(body);
    }

    public static int Serialize(Stream stream, ReadOnlyMemory<byte> discriminator, ReadOnlyMemory<byte> body,
        Guid? correlationId = null, DateTime? timestamp = null)
    {
        var total = 0;
        var buffer = new byte[16].AsSpan();

        // discriminator
        var size = VariableLengthEncoder.EncodeUint(buffer, (uint)discriminator.Length);
        stream.Write(buffer[..size]);
        stream.Write(discriminator.Span);
        total += size + discriminator.Span.Length;

        // body
        size = VariableLengthEncoder.EncodeUint(buffer, (uint)body.Length);
        stream.Write(buffer[..size]);
        stream.Write(body.Span);
        total += size + body.Span.Length;

        // correlationId
        if (correlationId.HasValue)
        {
            size = VariableLengthEncoder.EncodeUint(buffer, (uint)MessageRecordFields.CorrelationId);
            stream.Write(buffer[..size]);
            correlationId.Value.TryWriteBytes(buffer);
            stream.Write(buffer[..16]);
            total += size + 16;
        }

        //timestamp
        if (timestamp.HasValue)
        {
            size = VariableLengthEncoder.EncodeUint(buffer, (uint)MessageRecordFields.Timestamp);
            stream.Write(buffer[..size]);
            total += size;
            size = VariableLengthEncoder.EncodeUlong(buffer,
                (ulong)((DateTimeOffset)timestamp.Value).ToUnixTimeMilliseconds());
            stream.Write(buffer[..size]);
            total += size;
        }

        return total;
    }
}