namespace GymAppCore.Models.Entities;

public class SimpleExercise
{
    protected SimpleExercise()
    {
    }
    public SimpleExercise(DateTime date, string? description, Guid userId, Exercise exercise, List<Series> series)
    {
        Date = date;
        Description = description;
        UserId = userId;
        Exercise = exercise;
        Series = series;
    }
    public Guid Id { get; set; } = Guid.NewGuid();
    public Exercise Exercise { get; set; }
    public Guid ExerciseId { get; set; }
    public DateTime Date { get; set; }
    public List<Series> Series { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
    public string? Description { get; set; }
}