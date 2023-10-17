using GyMeApplication.Models.Series;

namespace GyMeApplication.Models.SimpleExercise;

public class PutSimpleExerciseDto
{
    public string? Description { get; set; }
    public IEnumerable<PutSeriesDto> PutSeriesDto { get; set; }
}