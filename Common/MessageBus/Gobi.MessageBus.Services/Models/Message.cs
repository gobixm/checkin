namespace Gobi.MessageBus.Services.Models;

public record Message<T>(
    T Body,
    Guid? CorrelationId,
    DateTime? Timestamp
);