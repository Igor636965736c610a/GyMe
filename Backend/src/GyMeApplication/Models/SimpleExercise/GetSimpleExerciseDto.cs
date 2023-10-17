using GyMeApplication.Models.ReactionsAndComments;
using GyMeApplication.Models.Series;
using GyMeApplication.Models.Exercise;

namespace GyMeApplication.Models.SimpleExercise;

public class GetSimpleExerciseDto
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; set; }
    public string ExercisesTypeDto { get; set; }
    public DateTime Date { get; set; }
    public Guid UserId { get; set; }
    public IEnumerable<GetSeriesDto> Series { get; set; }
    public IEnumerable<GetReactionDto> FirstThreeReactionsDto { get; set; }
    public int ReactionsCount { get; set; }
    public int CommentsCount { get; set; }
    public string? Description { get; set; }
    public int? MaxRep { get; set; }
    public int? Score { get; set; }
    public int? NumberOfRepetitions { get; set; }
    public int? NumberOfSeries { get; set; }
    public int? SumOfKilograms { get; set; }
    public int? AverageNumberOfRepetitionsPerSeries { get; set; }
    public int? AverageWeight { get; set; }
}