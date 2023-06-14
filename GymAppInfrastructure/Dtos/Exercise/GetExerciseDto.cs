using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Series;
using GymAppInfrastructure.Dtos.SimpleExercise;

namespace GymAppInfrastructure.Dtos.Exercise;

public class GetExerciseDto
{
    public Guid Id { get; set; }
    public ExercisesType ExercisesType { get; set; }
    public GetSeriesDto? MaxRep { get; set; }
}