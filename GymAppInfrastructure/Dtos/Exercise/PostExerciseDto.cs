using GymAppCore.Models.Entities;

namespace GymAppInfrastructure.Dtos.Exercise;

public class PostExerciseDto
{
    public ExercisesType ExercisesType { get; set; }
    public int? Position { get; set; }
}