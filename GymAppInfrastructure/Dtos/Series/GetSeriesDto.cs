﻿namespace GymAppInfrastructure.Dtos.Series;

public class GetSeriesDto
{
    public Guid Id { get; set; }
    public int NumberOfRepetitions { get; set; }
    public int NumberOfSeries { get; set; }
}