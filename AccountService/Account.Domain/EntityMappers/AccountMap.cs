using Checkin.AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkin.AccountService.Domain.EntityMappers;

public sealed class AccountMap : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> entity)
    {
        entity.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        entity.HasIndex(x => x.Login)
            .IsUnique();
        entity.Property(x => x.Created).HasDefaultValueSql("current_timestamp at time zone 'utc'");
    }
}