namespace GyMeCore.Models.Entities;

public class Exercise
{
    protected Exercise()
    {
    }
    public Exercise(string exercisesType, int position, Guid userId)
    {
        ExercisesType = exercisesType;
        Position = position;
        UserId = userId;
    }
    public Guid Id { get; set; } = Guid.NewGuid();
    public User User { get; set; }
    public Guid UserId { get; set; }
    public string ExercisesType { get; set; }
    public List<SimpleExercise> ConcreteExercise { get; set; } = new();
    public int Position { get; set; }
}