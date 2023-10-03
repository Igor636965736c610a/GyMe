using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GyMeCore.Models.Entities.Configurations;

public class CommentConfig : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Message).IsRequired().HasMaxLength(EntitiesConfig.CommentConf.MessageMaxLenght);
        builder.Property(x => x.TimeStamp).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.SimpleExerciseId).IsRequired();
        
        builder
            .HasMany(e => e.CommentReactions)
            .WithOne(e => e.Comment)
            .HasForeignKey(e => e.CommentId);
    }
}