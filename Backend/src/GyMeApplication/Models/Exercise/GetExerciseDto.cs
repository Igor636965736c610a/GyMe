using GyMeApplication.Models.Series;
using GyMeCore.Models.Entities;
using GyMeApplication.Models.SimpleExercise;

namespace GyMeApplication.Models.Exercise;

public class GetExerciseDto
{
    public Guid Id { get; set; }
    public string ExercisesType { get; set; }
    public GetSeriesDto? MaxRep { get; set; }
    public int Position { get; set; }
}