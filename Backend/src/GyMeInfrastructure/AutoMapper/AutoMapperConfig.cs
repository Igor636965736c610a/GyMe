using AutoMapper;
using GyMeCore.Models.Entities;
using GyMeCore.Models.Results;
using GyMeInfrastructure.Models.Account;
using GyMeInfrastructure.Models.Exercise;
using GyMeInfrastructure.Models.Series;
using GyMeInfrastructure.Models.SimpleExercise;
using GyMeInfrastructure.Models.User;

namespace GyMeInfrastructure.AutoMapper;

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
                cfg.CreateMap<User, GetAccountInfModel>().ReverseMap();
                cfg.CreateMap<CommonFriendsResult, CommonFriendsResultDto>().ReverseMap();
            })
            .CreateMapper();
}