using AutoMapper;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Account;
using GymAppInfrastructure.Dtos.Exercise;
using GymAppInfrastructure.Dtos.Series;
using GymAppInfrastructure.Dtos.SimpleExercise;
using GymAppInfrastructure.Dtos.User;

namespace GymAppInfrastructure.AutoMapper;

public static class AutoMapperConfig
{
    public static IMapper Initialize()
        => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Exercise, GetExerciseDto>().ReverseMap();
                cfg.CreateMap<Series, GetSeriesDto>().ReverseMap();
                cfg.CreateMap<Series, BaseSeriesDto>().ReverseMap();
                cfg.CreateMap<SimpleExercise, GetSimpleExerciseDto>().ReverseMap();
                cfg.CreateMap<User, GetUserDto>().ReverseMap();
                cfg.CreateMap<User, ShowProfileDto>();
                cfg.CreateMap<User, GetAccountDto>().ReverseMap();
                cfg.CreateMap<ExercisesType, ExercisesTypeDto>().ReverseMap();
            })
            .CreateMapper();
}