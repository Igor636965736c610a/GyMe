﻿using System.Net.NetworkInformation;
using AutoMapper;
using FluentEmail.Core;
using GyMeCore.IRepo;
using GyMeCore.Models.Entities;
using GyMeInfrastructure.Exceptions;
using GyMeInfrastructure.IServices;
using GyMeInfrastructure.Models.Exercise;
using GyMeInfrastructure.Models.Series;

namespace GyMeInfrastructure.Services;

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
        var exercise = new Exercise(exerciseType, postExerciseDto.Position.Value, userIdFromJwt);

        UpdateExercisesPositionUp(exercise.Position, exercises);

        await _exerciseRepo.Create(exercise);
        await _exerciseRepo.Update(exercises);
    }

    public async Task Update(Guid exerciseId, PutExerciseDto putExerciseDto)
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

        var maxRepSeries = await _exerciseRepo.GetMaxRep(exerciseId);
        GetSeriesDto? maxRepSeriesDto = null;
        if (maxRepSeries is not null)
            maxRepSeriesDto = _mapper.Map<Series, GetSeriesDto>(maxRepSeries);
        var exerciseDto = new GetExerciseDto()
        {
            Id = exercise.Id,
            ExercisesType = exercise.ExercisesType,
            MaxRep = maxRepSeriesDto,
        };

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
        var exercisesDto = exercises.Select(x => new GetExerciseDto()
        {
            Id = x.Id,
            ExercisesType = x.ExercisesType,
            MaxRep = _mapper.Map<Series, GetSeriesDto>(maxReps[x.Id])
        });

        return exercisesDto;
    }

    private static void UpdateExercisesPositionUp(int position, IEnumerable<Exercise> exercises)
        => exercises.Where(x => x.Position >= position).ForEach(x => x.Position += 1);
    
    private static void UpdateExercisesPositionDown(int position, IEnumerable<Exercise> exercises)
        => exercises.Where(x => x.Position >= position).ForEach(x => x.Position -= 1);
}