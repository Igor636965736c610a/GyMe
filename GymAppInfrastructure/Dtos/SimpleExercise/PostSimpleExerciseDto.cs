using GymAppCore.Models.Entities;

namespace GymAppInfrastructure.Dtos.SimpleExercise;

public class PostSimpleExerciseDto
{
    public ExercisesType ExercisesType { get; set; }
    public string? Series { get; set; }
    public string? Description { get; set; }
}