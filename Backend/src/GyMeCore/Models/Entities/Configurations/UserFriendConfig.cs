﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GyMeCore.Models.Entities.Configurations;

public class UserFriendConfig : IEntityTypeConfiguration<UserFriend>
{
    public void Configure(EntityTypeBuilder<UserFriend> builder)
    {
        builder.HasKey(x => new { x.UserId, x.FriendId });
        builder.Property(p => p.FriendStatus).IsRequired();
        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.FriendId).IsRequired();
        
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