using GymAppCore.Models.Entities;

namespace GymAppInfrastructure.Models.Exercise;

public class PostExerciseDto
{
    public ExercisesTypeDto ExercisesType { get; set; }
    public int? Position { get; set; }
}