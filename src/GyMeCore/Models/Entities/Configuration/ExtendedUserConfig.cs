using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymAppCore.Models.Entities.Configuration;

public class ExtendedUserConfig : IEntityTypeConfiguration<ExtendedUser>
{
    public void Configure(EntityTypeBuilder<ExtendedUser> builder)
    {
        builder.Property(x => x.Gender).IsRequired();
        builder.Property(x => x.ProfilePicture).IsRequired();
        
        builder.HasKey(x => x.UserId);
        builder
            .HasOne(x => x.User)
            .WithOne(x => x.ExtendedUser)
            .HasForeignKey<ExtendedUser>(x => x.UserId);
    }
}