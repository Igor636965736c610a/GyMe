using GymAppCore.Models.Entities;

namespace GymAppInfrastructure.Dtos.Exercise;

public class GetExerciseDto
{
    public ExercisesType ExercisesType { get; set; }
    public List<SimpleExercise.GetSimpleExerciseDto> ConcreteExercise { get; set; } = new();
}