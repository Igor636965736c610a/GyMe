using GyMeCore.Models.Entities;
using GyMeInfrastructure.Models.Series;

namespace GyMeInfrastructure.Models.SimpleExercise;

public class PostSimpleExerciseDto
{
    public Guid ExerciseId { get; set; }
    public IEnumerable<PostSeriesDto> SeriesDto { get; set; }
    public string? Description { get; set; }
}