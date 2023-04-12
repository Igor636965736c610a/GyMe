using AutoMapper;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.SimpleExercise;
using GymAppInfrastructure.Exceptions;
using GymAppInfrastructure.IServices;


namespace GymAppInfrastructure.Services;

public class SimpleExerciseService : ISimpleExerciseService
{
    private readonly ISimpleExerciseRepo _simpleExerciseRepo;
    private readonly IMapper _mapper;
    private readonly IExerciseRepo _exerciseRepo;
    private readonly ISeriesRepo _seriesRepo;
    public SimpleExerciseService(ISimpleExerciseRepo simpleExerciseRepo, IMapper mapper, IExerciseRepo exerciseRepo, ISeriesRepo seriesRepo)
    {
        _simpleExerciseRepo = simpleExerciseRepo;
        _mapper = mapper;
        _exerciseRepo = exerciseRepo;
        _seriesRepo = seriesRepo;
    }
    
    public async Task CreateSimpleExercise(PostSimpleExerciseDto postSimpleExerciseDto, Guid userId)
    {
        var exercise = await _exerciseRepo.Get(postSimpleExerciseDto.ExercisesType, userId);
        if (exercise is null)
            throw new InvalidOperationException("Exercise does not exist");
        var simpleExercise = new SimpleExercise(DateTime.UtcNow, postSimpleExerciseDto.Description, userId, exercise, postSimpleExerciseDto.Series);
        var series = UtilsServices.SeriesFromString(postSimpleExerciseDto.Series, simpleExercise);
        simpleExercise.Series = series;
        
        if (!await _simpleExerciseRepo.Create(simpleExercise))
            throw new SaveChangesDbException("something went wrong while saving database changes");
    }

    public async Task UpdateSimpleExercise(Guid userId, Guid id, PutSimpleExerciseDto putExerciseDto)
    {
        var simpleExercise = await _simpleExerciseRepo.Get(id);
        if (simpleExercise is null)
            throw new InvalidOperationException("Not Found");
        if (simpleExercise.UserId != userId)
            throw new ForbiddenException("You do not have access to this data");
        var series = UtilsServices.SeriesFromString(putExerciseDto.Series, simpleExercise);
        simpleExercise.Description = putExerciseDto.Description;
        simpleExercise.Date = putExerciseDto.Date;
        simpleExercise.SeriesString = putExerciseDto.Series;
        simpleExercise.Series = series;
        
        if(!await _simpleExerciseRepo.Update(simpleExercise))
            throw new SaveChangesDbException("something went wrong while saving database changes");
    }

    public async Task RemoveSimpleExercise(Guid userId, Guid id)
    {
        var simpleExercise = await _simpleExerciseRepo.Get(id);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");
        if (simpleExercise.UserId != userId)
            throw new ForbiddenException("You do not have access to this data");

        if(!await _simpleExerciseRepo.Remove(simpleExercise))
            throw new SaveChangesDbException("something went wrong while saving database changes");
    }

    public async Task<GetSimpleExerciseDto> GetSimpleExercise(Guid userId, Guid id)
    {
        var simpleExercise = await _simpleExerciseRepo.Get(id);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");
        if (simpleExercise.UserId != userId)
            throw new ForbiddenException("You do not have access to this data");

        var simpleExerciseDto = _mapper.Map<SimpleExercise, GetSimpleExerciseDto>(simpleExercise);

        return simpleExerciseDto;
    }

    public async Task<IEnumerable<GetSimpleExerciseDto>> GetSimpleExercises(Guid userId)
    {
        var simpleExercises = await _simpleExerciseRepo.GetAll(userId);
        var simpleExercisesDto =
            _mapper.Map<IEnumerable<SimpleExercise>, IEnumerable<GetSimpleExerciseDto>>(simpleExercises);

        return simpleExercisesDto;
    }
}