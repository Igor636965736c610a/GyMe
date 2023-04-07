using AutoMapper;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.SimpleExercise;
using GymAppInfrastructure.IServices;


namespace GymAppInfrastructure.Services;

public class SimpleExerciseService : ISimpleExerciseService
{
    private readonly ISimpleExerciseRepo _simpleExerciseRepo;
    private readonly IMapper _mapper;
    private readonly IExerciseRepo _exerciseRepo;
    public SimpleExerciseService(ISimpleExerciseRepo simpleExerciseRepo, IMapper mapper, IExerciseRepo exerciseRepo)
    {
        _simpleExerciseRepo = simpleExerciseRepo;
        _mapper = mapper;
        _exerciseRepo = exerciseRepo;
    }
    
    public async Task CreateSimpleExercise(PostSimpleExerciseDto postSimpleExerciseDto, User user)
    {
        var exercise = await _exerciseRepo.Get(user, postSimpleExerciseDto.ExercisesType);
        if (exercise is null)
        {
            throw new NullReferenceException("Exercise does not exist");
        }
        var simpleExercise = new SimpleExercise(DateTime.Now, postSimpleExerciseDto.Description, user, exercise);
        var series = UtilsServices.SeriesFromString(postSimpleExerciseDto.Series, simpleExercise);
        simpleExercise.Series = series;
        await _simpleExerciseRepo.Create(simpleExercise);
    }

    public async Task UpdateSimpleExercise(User user, Guid id, PutSimpleExerciseDto putExerciseDto)
    {
        var simpleExercise = await _simpleExerciseRepo.Get(user, id);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");
        var series = UtilsServices.SeriesFromString(putExerciseDto.Series, simpleExercise);
        simpleExercise.Description = putExerciseDto.Description;
        simpleExercise.Date = putExerciseDto.Date;
        simpleExercise.Series = series;

        await _simpleExerciseRepo.Update(simpleExercise);
    }

    public async Task RemoveSimpleExercise(User user, Guid id)
    {
        var simpleExercise = await _simpleExerciseRepo.Get(user, id);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");

        await _simpleExerciseRepo.Remove(simpleExercise);
    }

    public async Task<GetSimpleExerciseDto> GetSimpleExercise(User user, Guid id)
    {
        var simpleExercise = await _simpleExerciseRepo.Get(user, id);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");

        var simpleExerciseDto = _mapper.Map<SimpleExercise, GetSimpleExerciseDto>(simpleExercise);

        return simpleExerciseDto;
    }

    public async Task<IEnumerable<GetSimpleExerciseDto>> GetSimpleExercises(User user)
    {
        var simpleExercises = await _simpleExerciseRepo.Get(user);
        var simpleExercisesDto =
            _mapper.Map<IEnumerable<SimpleExercise>, IEnumerable<GetSimpleExerciseDto>>(simpleExercises);

        return simpleExercisesDto;
    }
}