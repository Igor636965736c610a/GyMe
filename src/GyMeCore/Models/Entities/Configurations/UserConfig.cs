using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymAppCore.Models.Entities.Configurations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(EntitiesConfig.UserConf.FirstNameMaxLength);
        builder.Property(p => p.LastName).IsRequired().HasMaxLength(EntitiesConfig.UserConf.LastNameLength);
        builder.Property(p => p.UserName).IsRequired().HasMaxLength(EntitiesConfig.UserConf.UserNameMaxLength);
        builder.Property(p => p.Valid).IsRequired();
        builder.Property(p => p.AccountProvider).IsRequired().HasMaxLength(EntitiesConfig.UserConf.AccountProviderMaxLength);
        
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