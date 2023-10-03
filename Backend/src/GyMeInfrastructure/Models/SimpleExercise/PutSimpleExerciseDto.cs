using GyMeInfrastructure.Models.Series;

namespace GyMeInfrastructure.Models.SimpleExercise;

public class PutSimpleExerciseDto
{
    public string? Description { get; set; }
    public IEnumerable<PutSeriesDto> SeriesDto { get; set; }
}