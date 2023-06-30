using GymAppInfrastructure.Dtos.Exercise;

namespace GymAppApi.BodyRequest.Exercise;

public class PostExerciseBody
{
    public ExercisesTypeDto ExercisesType { get; set; }
    public int? Position { get; set; }
}