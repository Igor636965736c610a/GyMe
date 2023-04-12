using GymAppCore.Models.Entities;

namespace GymAppApi.BodyRequest.Exercise;

public class PostExerciseBody
{
    public ExercisesType ExercisesType { get; set; }
    public int? Position { get; set; }
}