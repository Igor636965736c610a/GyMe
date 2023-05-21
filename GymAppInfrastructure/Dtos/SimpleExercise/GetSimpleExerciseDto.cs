using GymAppInfrastructure.Dtos.Series;

namespace GymAppInfrastructure.Dtos.SimpleExercise;

public class GetSimpleExerciseDto
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; set; }
    public DateTime Date { get; set; }
    public IEnumerable<GetSeriesDto> Series { get; set; }
    public string? Description { get; set; }
    public string? SeriesString { get; set; }
    public int? MaxRep { get; set; }
    public int? Score { get; set; }
    public int? NumberOfRepetitions { get; set; }
    public int? NumberOfSeries { get; set; }
    public int? SumOfKilograms { get; set; }
    public int? AverageNumberOfRepetitionsPerSeries { get; set; }
}