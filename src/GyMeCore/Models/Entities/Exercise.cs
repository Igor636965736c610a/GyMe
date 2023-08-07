namespace GymAppCore.Models.Entities;

public class Exercise
{
    protected Exercise()
    {
    }
    public Exercise(ExercisesType exercisesType, int position, Guid userId)
    {
        ExercisesType = exercisesType;
        Position = position;
        UserId = userId;
    }
    public Guid Id { get; set; } = Guid.NewGuid();
    public User User { get; set; }
    public Guid UserId { get; set; }
    public ExercisesType ExercisesType { get; set; }
    public List<SimpleExercise> ConcreteExercise { get; set; } = new();
    public int Position { get; set; }
}