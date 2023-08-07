﻿using GymAppInfrastructure.Dtos.Series;

namespace GymAppInfrastructure.Dtos.SimpleExercise;

public class PutSimpleExerciseDto
{
    public string? Description { get; set; }
    public IEnumerable<PutSeriesDto> SeriesDto { get; set; }
    public string? Series { get; set; } 
}