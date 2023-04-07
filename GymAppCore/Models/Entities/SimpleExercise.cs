namespace GymAppCore.Models.Entities;

public class SimpleExercise
{
    protected SimpleExercise()
    {
    }
    public SimpleExercise(DateTime date, string? description, User user, Exercise exercise)
    {
        Date = date;
        Description = description;
        User = user;
        Exercise = exercise;
    }
    public Guid Id { get; set; } = Guid.NewGuid();
    public Exercise Exercise { get; set; }
    public DateTime Date { get; set; }
    public List<Series> Series { get; set; } = new();
    public User User { get; set; }
    public string? Description { get; set; }
}