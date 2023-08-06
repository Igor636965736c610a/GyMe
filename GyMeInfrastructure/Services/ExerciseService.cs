using System.Net.NetworkInformation;
using AutoMapper;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Exercise;
using GymAppInfrastructure.Dtos.Series;
using GymAppInfrastructure.Exceptions;
using GymAppInfrastructure.IServices;

namespace GymAppInfrastructure.Services;

internal class ExerciseService : IExerciseService
{
    private readonly IMapper _mapper;
    private readonly IExerciseRepo _exerciseRepo;
    private readonly IUserRepo _userRepo;
    private readonly IUserContextService _userContextService;
    
    public ExerciseService(IMapper autoMapper, IExerciseRepo exerciseRepo, IUserRepo userRepo, IUserContextService userContextService)
    {
        _mapper = autoMapper;
        _exerciseRepo = exerciseRepo;
        _userRepo = userRepo;
        _userContextService = userContextService;
    }
    
    public async Task Create(PostExerciseDto postExerciseDto)
    {
        var userIdFromJwt = _userContextService.GetUserId;
        
        if (postExerciseDto.Position is null)
            postExerciseDto.Position = 0;

        var exerciseType = (ExercisesType)postExerciseDto.ExercisesType;
        var existingExercise = await _exerciseRepo.Get(userIdFromJwt, exerciseType);
        if (existingExercise is not null)
            throw new InvalidOperationException("Exercise already exist");
        
        var exercises = await _exerciseRepo.GetAll(userIdFromJwt);
        var exercise = new Exercise(exerciseType, postExerciseDto.Position.Value, userIdFromJwt);

        var toUpdate = AddExercise(exercise, exercises);

        await _exerciseRepo.Create(exercise);
        await _exerciseRepo.Update(toUpdate);
    }

    public async Task Update(Guid exerciseId, PutExerciseDto putExerciseDto)
    {
        var userIdFromJwt = _userContextService.GetUserId;
        
        var exercise = await _exerciseRepo.Get(exerciseId);
        if (exercise is null)
            throw new InvalidOperationException("Not Found");
        if(exercise.UserId != userIdFromJwt)
            throw new ForbiddenException("You do not have the appropriate permissions");
        
        var exercises = await _exerciseRepo.GetAll(userIdFromJwt);
        exercises.Remove(exercise);
        exercise.Position = putExerciseDto.Position;
        AddExercise(exercise, exercises);

        await _exerciseRepo.Update(exercise);
        await _exerciseRepo.Update(exercises);
    }

    public async Task Remove(Guid exerciseId)
    {
        var userIdFromJwt = _userContextService.GetUserId;
        
        var exercises = await _exerciseRepo.GetAll(userIdFromJwt);
        var exercise = await _exerciseRepo.Get(exerciseId);
        if (exercise is null)
            throw new InvalidOperationException("Not Found");
        if (exercise.UserId != userIdFromJwt)
            throw new ForbiddenException("You do not have the appropriate permissions");
        
        var toUpdate = RemoveExercise(exercise, exercises);

        await _exerciseRepo.Remove(exercise);
        await _exerciseRepo.Update(toUpdate);
    }

    public async Task<GetExerciseDto> Get(Guid exerciseId)
    {
        var userIdFromJwt = _userContextService.GetUserId;
        
        var exercise = await _exerciseRepo.Get(exerciseId);
        if (exercise is null)
            throw new NullReferenceException("Not Found");
        
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, exercise.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var maxRepSeries = await _exerciseRepo.GetMaxRep(exerciseId);
        GetSeriesDto? maxRepSeriesDto = null;
        if (maxRepSeries is not null)
            maxRepSeriesDto = _mapper.Map<Series, GetSeriesDto>(maxRepSeries);
        var exerciseDto = new GetExerciseDto()
        {
            Id = exercise.Id,
            ExercisesType = _mapper.Map<ExercisesType, ExercisesTypeDto>(exercise.ExercisesType),
            MaxRep = maxRepSeriesDto,
        };

        return exerciseDto;
    }

    public async Task<IEnumerable<GetExerciseDto>> Get(Guid userId, int page, int size)
    {
        var userIdFromJwt = _userContextService.GetUserId;
        
        var owner = await _userRepo.Get(userId);
        if (owner is null)
            throw new InvalidOperationException("User does not exist");
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, userId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var exercises = await _exerciseRepo.GetAll(userId, page, size);

        var ids = exercises.Select(x => x.Id);
        var maxReps = await _exerciseRepo.GetMaxReps(ids);
        var exercisesDto = exercises.Select(x => new GetExerciseDto()
        {
            Id = x.Id,
            ExercisesType = _mapper.Map<ExercisesType, ExercisesTypeDto>(x.ExercisesType),
            MaxRep = _mapper.Map<Series, GetSeriesDto>(maxReps[x.Id])
        });

        return exercisesDto;
    }

    private static List<Exercise> AddExercise(Exercise exercise, List<Exercise> exercises)
    {
        List<Exercise> output = new();
        if (exercises.Count == 0)
            return exercises;
        if (exercise.Position < 0)
        {
            exercise.Position = 0;
        }

        if (exercise.Position > exercises.Count - 1)
        {
            exercise.Position = exercises.Count;
            return exercises;
        }

        foreach (var e in exercises)
        {
            if (e.Position >= exercise.Position)
            {
                e.Position++;
                output.Add(e);
            }
        }

        return output;
    }
    private static List<Exercise> RemoveExercise(Exercise exercise, List<Exercise> exercises)
    {
        List<Exercise> output = new();
        if (exercise.Position > exercises.Count - 1)
        {
            exercise.Position = exercises.Count;
            return exercises;
        }

        foreach (var e in exercises)
        {
            if (e.Position > exercise.Position)
            {
                e.Position--;
                output.Add(e);
            }
        }

        return output;
    }
}