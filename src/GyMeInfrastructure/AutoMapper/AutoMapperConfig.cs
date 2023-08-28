using AutoMapper;
using GymAppCore.Models.Entities;
using GymAppCore.Models.Results;
using GymAppInfrastructure.Models.Account;
using GymAppInfrastructure.Models.Exercise;
using GymAppInfrastructure.Models.Series;
using GymAppInfrastructure.Models.SimpleExercise;
using GymAppInfrastructure.Models.User;

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
                cfg.CreateMap<User, GetAccountInfModel>().ReverseMap();
                cfg.CreateMap<CommonFriendsResult, CommonFriendsResultDto>().ReverseMap();
                cfg.CreateMap<ExercisesType, ExercisesTypeDto>().ReverseMap();
            })
            .CreateMapper();
}