using GyMeApplication.Models.Series;
using GyMeCore.Models.Entities;

namespace GyMeApplication.Models.SimpleExercise;

public class PostSimpleExerciseDto
{
    public Guid ExerciseId { get; set; }
    public IEnumerable<PostSeriesDto> PostSeriesDto { get; set; }
    public string? Description { get; set; }
}