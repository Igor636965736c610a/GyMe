using GymAppInfrastructure.Dtos.Series;

namespace GymAppInfrastructure.Dtos.SimpleExercise;

public class GetSimpleExerciseDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<GetSeriesDto> Series { get; set; }
    public string? Description { get; set; }
}