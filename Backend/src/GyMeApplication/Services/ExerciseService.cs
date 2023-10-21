using FluentEmail.Core;
using GyMeApplication.Exceptions;
using GyMeApplication.IServices;
using GyMeApplication.Models.Exercise;
using GyMeApplication.Models.Series;
using GyMeApplication.MyMapper;
using GyMeCore.IRepo;
using GyMeCore.Models.Entities;

namespace GyMeApplication.Services;

internal class ExerciseService : IExerciseService
{
    private readonly IExerciseRepo _exerciseRepo;
    private readonly IUserRepo _userRepo;
    private readonly IUserContextService _userContextService;
    private readonly IGyMeMapper _gyMeMapper;

    public ExerciseService(IExerciseRepo exerciseRepo, IUserRepo userRepo, IUserContextService userContextService, IGyMeMapper gyMeMapper)
    {
        _exerciseRepo = exerciseRepo;
        _userRepo = userRepo;
        _userContextService = userContextService;
        _gyMeMapper = gyMeMapper;
    }
    
    public async Task<Guid> Create(PostExerciseDto postExerciseDto)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        postExerciseDto.Position ??= 0;

        var exerciseType = postExerciseDto.ExercisesTypeDto.ToStringFast();
        var existingExercise = await _exerciseRepo.Get(userIdFromJwt, exerciseType);
        if (existingExercise is not null)
            throw new InvalidOperationException("Exercise already exist");

        var exercises = await _exerciseRepo.GetAll(userIdFromJwt);
        if (postExerciseDto.Position < 0)
            postExerciseDto.Position = 0;
        if (postExerciseDto.Position > exercises.Count)
            postExerciseDto.Position = exercises.Count;
        
        var exercise = new Exercise(Guid.NewGuid(), exerciseType, postExerciseDto.Position.Value, userIdFromJwt);

        UpdateExercisesPositionUp(exercise.Position, exercises);

        await _exerciseRepo.Create(exercise);
        await _exerciseRepo.Update(exercises);
        return exercise.Id;
    }

    public async Task<GetExerciseDto> Update(Guid exerciseId, PutExerciseDto putExerciseDto)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var exercise = await _exerciseRepo.Get(exerciseId);
        if (exercise is null)
            throw new NullReferenceException("Not Found");
        if(exercise.UserId != userIdFromJwt)
            throw new ForbiddenException("You do not have the appropriate permissions");
        
        var exercises = await _exerciseRepo.GetAll(userIdFromJwt);
        if (putExerciseDto.Position < 0)
            putExerciseDto.Position = 0;
        if (putExerciseDto.Position > exercises.Count)
            putExerciseDto.Position = exercises.Count;
        exercises.Remove(exercise);
        
        UpdateExercisesPositionDown(exercise.Position, exercises);
        UpdateExercisesPositionUp(putExerciseDto.Position, exercises);
        exercise.Position = putExerciseDto.Position;

        await _exerciseRepo.Update(exercise);
        await _exerciseRepo.Update(exercises);
        
        var maxRepSeriesDto = await MaxRepSeriesDto(exerciseId);
        
        return _gyMeMapper.GetExerciseDtoMap(exercise, maxRepSeriesDto);
    }

    public async Task Remove(Guid exerciseId)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var exercise = await _exerciseRepo.Get(exerciseId);
        if (exercise is null)
            throw new NullReferenceException("Not Found");
        if (exercise.UserId != userIdFromJwt)
            throw new ForbiddenException("You do not have the appropriate permissions");
        
        await _exerciseRepo.Remove(exercise);
        var exercises = await _exerciseRepo.GetAll(userIdFromJwt);
        
        UpdateExercisesPositionDown(exercise.Position, exercises);
        
        await _exerciseRepo.Update(exercises);
    }

    public async Task<GetExerciseDto> Get(Guid exerciseId)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var exercise = await _exerciseRepo.Get(exerciseId);
        if (exercise is null)
            throw new NullReferenceException("Not Found");
        
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, exercise.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var maxRepSeriesDto = await MaxRepSeriesDto(exerciseId);

        var exerciseDto = _gyMeMapper.GetExerciseDtoMap(exercise, maxRepSeriesDto);
        
        return exerciseDto;
    }


    public async Task<IEnumerable<GetExerciseDto>> Get(Guid userId, int page, int size)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var owner = await _userRepo.GetOnlyValid(userId);
        if (owner is null)
            throw new InvalidOperationException("User does not exist");
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, userId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var exercises = await _exerciseRepo.GetAll(userId, page, size);

        var ids = exercises.Select(x => x.Id);
        var maxReps = await _exerciseRepo.GetMaxReps(ids);
        var maxRepsDto = maxReps.ToDictionary(x => x.Key, x 
            => x.Value is not null ? _gyMeMapper.GetSeriesDtoMap(x.Value) : null);

        var exercisesDto = exercises.Select(x => _gyMeMapper.GetExerciseDtoMap(x, maxRepsDto[x.Id]));

        return exercisesDto;
    }

    private static void UpdateExercisesPositionUp(int position, IEnumerable<Exercise> exercises)
        => exercises.Where(x => x.Position >= position).ForEach(x => x.Position += 1);
    
    private static void UpdateExercisesPositionDown(int position, IEnumerable<Exercise> exercises)
        => exercises.Where(x => x.Position >= position).ForEach(x => x.Position -= 1);
    
    private async Task<GetSeriesDto?> MaxRepSeriesDto(Guid exerciseId)
    {
        var maxRepSeries = await _exerciseRepo.GetMaxRep(exerciseId);
        GetSeriesDto? maxRepSeriesDto = null;
        if (maxRepSeries is not null)
            maxRepSeriesDto = _gyMeMapper.GetSeriesDtoMap(maxRepSeries);
        return maxRepSeriesDto;
    }
}
