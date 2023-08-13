using GymAppCore.Models.Entities;
using GymAppInfrastructure.Models.Series;

namespace GymAppInfrastructure.MyMapper;

public static class GetSeriesDtoMap
{
    public static GetSeriesDto Map(Series series)
    {
        return new GetSeriesDto()
        {
            Id = series.Id,
            NumberOfRepetitions = series.NumberOfRepetitions,
            Weight = series.Weight
        };
    }
}