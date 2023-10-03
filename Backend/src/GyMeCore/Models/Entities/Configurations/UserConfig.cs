using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GyMeCore.Models.Entities.Configurations;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(EntitiesConfig.UserConf.FirstNameMaxLength);
        builder.Property(p => p.LastName).IsRequired().HasMaxLength(EntitiesConfig.UserConf.LastNameLength);
        builder.Property(p => p.UserName).IsRequired().HasMaxLength(EntitiesConfig.UserConf.UserNameMaxLength);
        builder.Property(p => p.Valid).IsRequired();
        builder.Property(p => p.AccountProvider).IsRequired().HasMaxLength(EntitiesConfig.UserConf.AccountProviderMaxLength);

        builder
            .HasMany(e => e.SimpleExercises)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);

        builder
            .HasMany(e => e.Exercises)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);

        builder
            .HasMany(e => e.Reactions)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);

        builder
            .HasMany(e => e.Comments)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);

        builder
            .HasMany(e => e.CommentReactions)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);

        builder
            .HasOne(x => x.ExtendedUser)
            .WithOne(x => x.User)
            .HasForeignKey<ExtendedUser>(x => x.UserId);

        builder
            .HasOne(x => x.SetResourcesAddresses)
            .WithOne(x => x.User)
            .HasForeignKey<ResourcesAddresses>(x => x.UserId);
    }
}