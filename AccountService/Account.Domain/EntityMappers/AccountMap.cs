using AccountDomain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountDomain.EntityMappers;

public sealed class AccountMap : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> entity)
    {
        entity.Property(x => x.AccountId)
            .ValueGeneratedOnAdd();
        entity.Property(x => x.Created).HasDefaultValueSql("current_timestamp at time zone 'utc'");
    }
}