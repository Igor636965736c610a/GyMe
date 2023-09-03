using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymAppCore.Models.Entities.Configurations;

public class SeriesConfig : IEntityTypeConfiguration<Series>
{
    public void Configure(EntityTypeBuilder<Series> builder)
    {
        builder.Property(x => x.Weight).IsRequired().HasMaxLength(EntitiesConfig.SeriesConf.WeightMaxLenght);
        builder.Property(x => x.NumberOfRepetitions).IsRequired().HasMaxLength(EntitiesConfig.SeriesConf.NumberOfRepetitionsMaxLenght);
    }
}