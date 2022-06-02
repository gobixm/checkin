using Checkin.AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkin.AccountService.Domain.EntityMappers;

public sealed class AccountFriendMap : IEntityTypeConfiguration<AccountFriend>
{
    public void Configure(EntityTypeBuilder<AccountFriend> entity)
    {
        entity.HasKey(x => new { x.AccountId, x.FriendId });
        entity.Property(x => x.Created).HasDefaultValueSql("current_timestamp at time zone 'utc'");
        entity.HasOne(x => x.Account).WithMany(x => x.Friends).HasForeignKey(x => x.AccountId);
        entity.HasOne(x => x.Friend).WithMany().HasForeignKey(x => x.FriendId);
    }
}