using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymAppCore.Models.Entities.Configuration;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(EntitiesConfig.User.FirstNameMaxLength);
        builder.Property(p => p.LastName).IsRequired().HasMaxLength(EntitiesConfig.User.LastNameLength);
        builder.Property(p => p.UserName).HasMaxLength(EntitiesConfig.User.UserNameMaxLength);
        builder.Property(p => p.PrivateAccount).IsRequired();
        builder.Property(p => p.Valid).IsRequired();
        builder.Property(p => p.AccountProvider).IsRequired().HasMaxLength(EntitiesConfig.User.AccountProviderMaxLength);
        
        builder.HasMany(e => e.SimpleExercises)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(e => e.Id);
        
        builder.HasMany(e => e.Exercises)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(e => e.Id);
    }
}