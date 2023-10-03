using FluentValidation;
using GyMeCore.Models.Entities.Configurations;
using GyMeInfrastructure.Models.Series;

namespace GyMeInfrastructure.Models.Validations;

public class BaseSeriesDtoValidator : AbstractValidator<BaseSeriesDto>
{
    public BaseSeriesDtoValidator()
    {
        RuleFor(x => x.Weight).NotEmpty().Must(x => x is <= EntitiesConfig.SeriesConf.WeightMaxLenght and >= EntitiesConfig.SeriesConf.WeightMinLenght);
        RuleFor(x => x.NumberOfRepetitions).NotEmpty()
            .Must(x => x is <= EntitiesConfig.SeriesConf.NumberOfRepetitionsMaxLenght and >= EntitiesConfig.SeriesConf.NumberOfRepetitionsMinLenght);
    }
}