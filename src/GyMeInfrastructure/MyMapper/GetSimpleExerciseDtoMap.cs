using GymAppCore.Models.Entities;
using GymAppInfrastructure.Models.SimpleExercise;

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
            MaxRep = simpleExercise.Series.OrderByDescending(x => x.Weight).ThenByDescending(x => x.NumberOfRepetitions).First().Weight,
            Score = simpleExercise.Series.Sum(x => (int)Math.Round(x.Weight / (1.0278 - 0.0278 * x.NumberOfRepetitions), 2, MidpointRounding.AwayFromZero)),
            NumberOfRepetitions = simpleExercise.Series.Sum(x => x.NumberOfRepetitions),
            NumberOfSeries = simpleExercise.Series.Count,
            SumOfKilograms = simpleExercise.Series.Sum(x => x.Weight),
            AverageNumberOfRepetitionsPerSeries = (int)Math.Round(simpleExercise.Series.Average(x => x.NumberOfRepetitions), 2, MidpointRounding.AwayFromZero),
            AverageWeight = (int)Math.Round(simpleExercise.Series.Average(x => x.Weight), 2, MidpointRounding.AwayFromZero)
        };
    }
}