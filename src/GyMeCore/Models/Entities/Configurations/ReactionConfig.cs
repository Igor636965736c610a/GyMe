using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymAppCore.Models.Entities.Configurations;

public class ReactionConfig : IEntityTypeConfiguration<Reaction>
{
    public void Configure(EntityTypeBuilder<Reaction> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.ReactionType).IsRequired();
        builder.Property(x => x.TimeStamp).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.SimpleExerciseId).IsRequired();
    }
}