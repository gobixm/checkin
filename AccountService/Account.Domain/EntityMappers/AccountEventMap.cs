using Checkin.AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkin.AccountService.Domain.EntityMappers;

public sealed class AccountEventMap : IEntityTypeConfiguration<AccountEvent>
{
    public void Configure(EntityTypeBuilder<AccountEvent> entity)
    {
        entity.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        entity.Property(x => x.Created).HasDefaultValueSql("current_timestamp at time zone 'utc'");
        entity.Property(x => x.Discriminator).HasConversion<int>();
    }
}