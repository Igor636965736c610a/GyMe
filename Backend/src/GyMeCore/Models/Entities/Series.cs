namespace GyMeCore.Models.Entities;

public class Series
{
    protected Series()
    {
    }
    public Series(SimpleExercise simpleExercise, int numberOfRepetitions, int weight)
    {
        NumberOfRepetitions = numberOfRepetitions;
        Weight = weight;
        SimpleExercise = simpleExercise;
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public int NumberOfRepetitions { get; set; }
    public int Weight { get; set; }
    public SimpleExercise SimpleExercise { get; set; }
    public Guid SimpleExerciseId { get; set; }
}