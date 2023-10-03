namespace GyMeInfrastructure.Models.Series;

public class GetSeriesDto
{
    public Guid Id { get; set; }
    public int NumberOfRepetitions { get; set; }
    public int Weight { get; set; }
}