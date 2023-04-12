namespace GymAppCore.Models.Entities;

public class SimpleExercise
{
    protected SimpleExercise()
    {
    }
    public SimpleExercise(DateTime date, string? description, Guid userId, Exercise exercise, string? seriesString)
    {
        Date = date;
        Description = description;
        UserId = userId;
        Exercise = exercise;
        SeriesString = seriesString;
    }
    public Guid Id { get; set; } = Guid.NewGuid();
    public Exercise Exercise { get; set; }
    public DateTime Date { get; set; }
    public string? SeriesString { get; set; }
    public List<Series> Series { get; set; } = new();
    public User User { get; set; }
    public Guid UserId { get; set; }
    public string? Description { get; set; }
}