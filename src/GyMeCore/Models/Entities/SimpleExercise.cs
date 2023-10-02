namespace GymAppCore.Models.Entities;

public class SimpleExercise
{
    protected SimpleExercise()
    {
    }
    public SimpleExercise(Guid userId, Exercise exercise, List<Series> series, ExercisesType exerciseType, string? description)
    {
        Description = description;
        UserId = userId;
        Exercise = exercise;
        Series = series;
        ExerciseType = exerciseType;
    }
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Description { get; set; }
    public ExercisesType ExerciseType { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public Exercise Exercise { get; set; }
    public Guid ExerciseId { get; set; }
    public List<Series> Series { get; set; }
    public List<Reaction> Reactions { get; set; }
    public List<Comment> Comments { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
}