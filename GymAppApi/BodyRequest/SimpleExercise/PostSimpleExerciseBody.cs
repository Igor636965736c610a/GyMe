﻿using GymAppCore.Models.Entities;

namespace GymAppApi.BodyRequest.SimpleExercise;

public class PostSimpleExerciseBody
{
    public string ExerciseId { get; set; }
    public string? Series { get; set; }
    public string? Description { get; set; }
}