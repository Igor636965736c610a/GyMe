using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GyMeCore.Models.Entities.Configurations;

public class CommentReactionConfig : IEntityTypeConfiguration<CommentReaction>
{
    public void Configure(EntityTypeBuilder<CommentReaction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Emoji).IsRequired();
        builder.Property(x => x.TimeStamp).IsRequired();
        builder.Property(x => x.CommentId).IsRequired();
        builder.Property(x => x.UserId);
    }
}