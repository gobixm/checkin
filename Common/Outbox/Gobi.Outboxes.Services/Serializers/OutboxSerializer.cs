using System.Text.Json;
using Gobi.Outboxes.Domain.Models;

namespace Gobi.Outboxes.Services.Serializers;

public sealed class OutboxSerializer : IOutboxSerializer
{
    public OutboxEvent Serialize<T>(T body)
    {
        return new()
        {
            Body = JsonSerializer.SerializeToUtf8Bytes(body),
            Discriminator = typeof(T).Name
        };
    }
    
    public OutboxEvent Serialize<T>(T body, string discriminator)
    {
        return new()
        {
            Body = JsonSerializer.SerializeToUtf8Bytes(body),
            Discriminator = discriminator
        };
    }
}