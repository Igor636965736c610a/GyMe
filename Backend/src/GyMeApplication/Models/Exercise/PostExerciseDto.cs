using GyMeCore.Models.Entities;

namespace GyMeApplication.Models.Exercise;

public class PostExerciseDto
{
    public ExercisesTypeDto ExercisesTypeDto { get; set; }
    public int? Position { get; set; }
}