using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GyMeCore.Models.Entities.Configurations;

public class ExtendedUserConfig : IEntityTypeConfiguration<ExtendedUser>
{
    public void Configure(EntityTypeBuilder<ExtendedUser> builder)
    {
        builder.HasKey(x => x.UserId);
        builder.Property(x => x.Gender).IsRequired();
        builder.Property(x => x.ProfilePictureUrl).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(EntitiesConfig.ExtendedUserConf.DescriptionMaxLenght);
    }
}