namespace GymAppCore.Models.Entities;

public class Exercise
{
    protected Exercise()
    {
    }
    public Exercise(ExercisesType exercisesType, int position, User user)
    {
        ExercisesType = exercisesType;
        Position = position;
        User = user;
    }
    public Guid Id { get; set; } = Guid.NewGuid();
    public User User { get; set; }
    public bool Shown { get; set; } = true;
    public ExercisesType ExercisesType { get; set; }
    public List<SimpleExercise> ConcreteExercise { get; set; } = new();
    public int Position { get; set; }
}