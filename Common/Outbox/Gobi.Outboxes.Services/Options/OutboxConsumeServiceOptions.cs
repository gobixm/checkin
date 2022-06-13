namespace Gobi.Outboxes.Services.Options;

public sealed class OutboxConsumeServiceOptions
{
    public TimeSpan BatchInterval { get; set; } = TimeSpan.FromSeconds(5);
    public int MaxBatchSize { get; set; } = 1000;
}