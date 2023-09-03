using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymAppCore.Models.Entities.Configurations;

public class SimpleExerciseConfig : IEntityTypeConfiguration<SimpleExercise>
{
    public void Configure(EntityTypeBuilder<SimpleExercise> builder)
    {
        builder.Property(p => p.Id).IsRequired();
        //builder.Property(p => p.ExerciseId).IsRequired();
        builder.Property(p => p.Date).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(EntitiesConfig.SimpleExerciseConf.DescriptionMaxLength);
        
        builder.HasMany(e => e.Series)
                .WithOne(e => e.SimpleExercise)
                .HasForeignKey(e => e.SimpleExerciseId)
                .HasPrincipalKey(e => e.Id);
    }
}