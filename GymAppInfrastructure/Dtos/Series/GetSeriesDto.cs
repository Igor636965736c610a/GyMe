namespace GymAppInfrastructure.Dtos.Series;

public class GetSeriesDto
{
    public Guid Id = Guid.NewGuid();
    public int NumberOfRepetitions { get; set; }
    public int NumberOfSeries { get; set; }
}