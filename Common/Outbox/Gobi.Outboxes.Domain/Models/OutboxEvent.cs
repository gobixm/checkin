namespace Gobi.Outboxes.Domain.Models;

public sealed class OutboxEvent
{
    public byte[] Body { get; set; } = null!;
    public DateTime Created { get; set; }
    public string Discriminator { get; set; } = null!;
    public int Offset { get; set; }
}