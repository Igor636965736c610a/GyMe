using AutoMapper;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Exercise;
using GymAppInfrastructure.Dtos.Series;

namespace GymAppInfrastructure.AutoMapper;

public static class AutoMapperConfig
{
    public static IMapper Initialize()
        => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Exercise, GetExerciseDto>().ReverseMap();
                cfg.CreateMap<IEnumerable<Exercise>, IEnumerable<GetExerciseDto>>();
                cfg.CreateMap<Series, GetExerciseDto>().ReverseMap();
                cfg.CreateMap<IEnumerable<Series>, IEnumerable<GetExerciseDto>>();
                cfg.CreateMap<SimpleExercise, Dtos.SimpleExercise.GetSimpleExerciseDto>().ReverseMap();
                cfg.CreateMap<IEnumerable<SimpleExercise>, IEnumerable<GetSeriesDto>>();
                cfg.CreateMap<User, GetUserDto>().ReverseMap();
            })
            .CreateMapper();
}