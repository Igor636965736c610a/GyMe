using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymAppCore.Models.Entities.Configurations;

public class CommentConfig : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasMany(e => e.CommentReactions)
            .WithOne(e => e.Comment)
            .HasForeignKey(e => e.CommentId)
            .HasPrincipalKey(e => e.Id);
    }
}