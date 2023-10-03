using GymAppCore.Models.Entities;
using GymAppInfrastructure.Models.SimpleExercise;
using GymAppInfrastructure.Models.Series;

namespace GymAppInfrastructure.Models.Exercise;

public class GetExerciseDto
{
    public Guid Id { get; set; }
    public string ExercisesType { get; set; }
    public GetSeriesDto? MaxRep { get; set; }
}