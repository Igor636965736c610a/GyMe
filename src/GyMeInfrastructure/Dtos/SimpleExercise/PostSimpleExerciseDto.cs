using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Series;

namespace GymAppInfrastructure.Dtos.SimpleExercise;

public class PostSimpleExerciseDto
{
    public Guid ExerciseId { get; set; }
    public IEnumerable<PostSeriesDto> SeriesDto { get; set; }
    public string? Description { get; set; }
}