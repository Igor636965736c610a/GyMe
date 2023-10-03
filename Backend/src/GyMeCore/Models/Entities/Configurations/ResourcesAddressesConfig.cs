using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymAppCore.Models.Entities.Configurations;

public class ResourcesAddressesConfig : IEntityTypeConfiguration<ResourcesAddresses>
{
    public void Configure(EntityTypeBuilder<ResourcesAddresses> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.ReactionImageUrl).IsRequired();
    }
}