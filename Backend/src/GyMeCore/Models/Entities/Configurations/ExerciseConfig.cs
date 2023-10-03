using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GyMeCore.Models.Entities.Configurations;

public class ExerciseConfig : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.UserId).IsRequired();
        builder.Property(p => p.ExercisesType).IsRequired();
        builder.Property(p => p.Position).IsRequired();

        builder
            .HasMany(e => e.ConcreteExercise)
            .WithOne(e => e.Exercise)
            .HasForeignKey(e => e.ExerciseId);
    }
}