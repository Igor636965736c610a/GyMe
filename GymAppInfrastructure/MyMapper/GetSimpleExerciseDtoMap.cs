using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.SimpleExercise;

namespace GymAppInfrastructure.MyMapper;

public static class GetSimpleExerciseDtoMap
{
    public static GetSimpleExerciseDto Map(SimpleExercise simpleExercise)
    {
        return new GetSimpleExerciseDto()
        {
            Id = simpleExercise.Id,
            Date = simpleExercise.Date,
            ExerciseId = simpleExercise.ExerciseId,
            Series = simpleExercise.Series.Select(GetSeriesDtoMap.Map),
            Description = simpleExercise.Description,
            SeriesString = simpleExercise.SeriesString
        };
    }
}