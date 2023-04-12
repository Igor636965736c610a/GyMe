﻿using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.SimpleExercise;

namespace GymAppInfrastructure.Dtos.Exercise;

public class GetExerciseDto
{
    public ExercisesType ExercisesType { get; set; }
    public IEnumerable<GetSimpleExerciseDto> ConcreteExercise { get; set; } 
}