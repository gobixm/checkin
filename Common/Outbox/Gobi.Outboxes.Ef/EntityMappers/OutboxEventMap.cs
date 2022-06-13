using Gobi.Outboxes.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gobi.Outboxes.Ef.EntityMappers;

public sealed class OutboxEventMap : IEntityTypeConfiguration<OutboxEvent>
{
    public void Configure(EntityTypeBuilder<OutboxEvent> entity)
    {
        entity.HasKey(x => x.Offset);
        entity.Property(x => x.Offset)
            .ValueGeneratedOnAdd();
        entity.Property(x => x.Created).HasDefaultValueSql("current_timestamp at time zone 'utc'");
        entity.Property(x => x.Discriminator);
        entity.Property(x => x.Body).HasColumnType("bytea");
    }
}