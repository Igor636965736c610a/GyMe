using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymAppCore.Models.Entities.Configurations;

public class ExtendedUserConfig : IEntityTypeConfiguration<ExtendedUser>
{
    public void Configure(EntityTypeBuilder<ExtendedUser> builder)
    {
        builder.Property(x => x.Gender).IsRequired();
        builder.Property(x => x.ProfilePictureUrl).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Premium).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(EntitiesConfig.ExtendedUserConf.DescriptionMaxLenght);
        builder.HasKey(x => x.UserId);
        
        builder
            .HasOne(x => x.User)
            .WithOne(x => x.ExtendedUser)
            .HasForeignKey<ExtendedUser>(x => x.UserId);
    }
}