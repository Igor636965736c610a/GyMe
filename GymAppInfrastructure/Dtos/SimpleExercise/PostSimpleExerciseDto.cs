using GymAppCore.Models.Entities;

namespace GymAppInfrastructure.Dtos.SimpleExercise;

public class PostSimpleExerciseDto
{
    public Guid ExerciseId { get; set; }
    public string? Series { get; set; }
    public string? Description { get; set; }
}