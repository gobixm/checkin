using Gobi.Outboxes.Domain.Models;

namespace Gobi.Outboxes.Services.Serializers;

public interface IOutboxSerializer
{
    OutboxEvent Serialize<T>(T body);
    OutboxEvent Serialize<T>(T body, string discriminator);
}