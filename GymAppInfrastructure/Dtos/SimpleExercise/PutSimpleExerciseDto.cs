using GymAppInfrastructure.Dtos.Series;

namespace GymAppInfrastructure.Dtos.SimpleExercise;

public class PutSimpleExerciseDto
{
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public string? Series { get; set; } 
}