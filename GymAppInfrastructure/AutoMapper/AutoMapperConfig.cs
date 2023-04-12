﻿using AutoMapper;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Exercise;
using GymAppInfrastructure.Dtos.Series;
using GymAppInfrastructure.Dtos.SimpleExercise;

namespace GymAppInfrastructure.AutoMapper;

public static class AutoMapperConfig
{
    public static IMapper Initialize()
        => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Exercise, GetExerciseDto>().ReverseMap();
                cfg.CreateMap<Series, GetSeriesDto>().ReverseMap();
                cfg.CreateMap<SimpleExercise, GetSimpleExerciseDto>().ReverseMap();
                cfg.CreateMap<User, GetUserDto>().ReverseMap();
            })
            .CreateMapper();
}