using GymAppInfrastructure.Dtos.Series;

namespace GymAppInfrastructure.Dtos.SimpleExercise;

public class GetSimpleExerciseDto
{
    public Guid Id = Guid.NewGuid();
    public DateTime Date { get; set; }
    public List<GetSeriesDto> SeriesDto { get; set; }
    public string? Description { get; set; }
}