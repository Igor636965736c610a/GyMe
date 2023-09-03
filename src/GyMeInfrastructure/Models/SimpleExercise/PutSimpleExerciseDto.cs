using GymAppInfrastructure.Models.Series;

namespace GymAppInfrastructure.Models.SimpleExercise;

public class PutSimpleExerciseDto
{
    public string? Description { get; set; }
    public IEnumerable<PutSeriesDto> SeriesDto { get; set; }
}