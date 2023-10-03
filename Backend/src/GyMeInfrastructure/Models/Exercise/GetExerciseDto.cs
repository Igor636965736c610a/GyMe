using GyMeCore.Models.Entities;
using GyMeInfrastructure.Models.SimpleExercise;
using GyMeInfrastructure.Models.Series;

namespace GyMeInfrastructure.Models.Exercise;

public class GetExerciseDto
{
    public Guid Id { get; set; }
    public string ExercisesType { get; set; }
    public GetSeriesDto? MaxRep { get; set; }
}