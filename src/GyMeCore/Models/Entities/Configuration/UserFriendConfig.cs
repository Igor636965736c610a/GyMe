using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymAppCore.Models.Entities.Configuration;

public class UserFriendConfig : IEntityTypeConfiguration<UserFriend>
{
    public void Configure(EntityTypeBuilder<UserFriend> builder)
    {
        builder.Property(p => p.FriendStatus).IsRequired();
        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.FriendId).IsRequired();
        
        builder.HasKey(x => new { x.UserId, x.FriendId });
        builder
            .HasOne(e => e.User)
            .WithMany(e => e.Friends)
            .HasForeignKey(e => e.UserId);
        builder
            .HasOne(e => e.Friend)
            .WithMany(e => e.InverseFriends)
            .HasForeignKey(e => e.FriendId);
    }
}