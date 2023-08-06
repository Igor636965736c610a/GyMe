using System.Collections;
using AutoMapper;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Exercise;
using GymAppInfrastructure.Exceptions;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Options;

namespace GymAppInfrastructure.Services;

internal class ChartService : IChartService
{
    private readonly IExerciseRepo _exerciseRepo;
    private readonly IUserRepo _userRepo;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public ChartService(IExerciseRepo exerciseRepo, IUserRepo userRepo, IMapper mapper, IUserContextService userContextService)
    {
        _exerciseRepo = exerciseRepo;
        _userRepo = userRepo;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task<IEnumerable<int>?> Get(Guid exerciseId, ChartOption option, int period)
    {
        var userIdFromJwt = _userContextService.GetUserId;
        
        var exercise = await _exerciseRepo.Get(exerciseId);
        if (exercise is null)
            return new List<int>();
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, exercise.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var calculate = GetCalculate(option);
        var chart = await _exerciseRepo.GetScore(exerciseId, period, calculate);

        return chart;
    }
    
    public async Task<IEnumerable<int>?> Get(Guid userUd, ExercisesTypeDto exercisesTypeDto, ChartOption option, int period)
    {
        var userIdFromJwt = _userContextService.GetUserId;
        
        var exerciseType = _mapper.Map<ExercisesTypeDto, ExercisesType>(exercisesTypeDto);
        var exercise = await _exerciseRepo.Get(userUd, exerciseType);
        if (exercise is null)
            return new List<int>();
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, exercise.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var calculate = GetCalculate(option);
        var chart = await _exerciseRepo.GetScore(exercise.Id, period, calculate);

        return chart;
    }

    public async Task<Dictionary<Guid, IEnumerable<int>>?> Get(Guid userId, IEnumerable<Guid> ids, ChartOption option, int period)
    {
        var userIdFromJwt = _userContextService.GetUserId;
        
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, userId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var calculate = GetCalculate(option);
        var charts = await _exerciseRepo.GetScores(ids, period, calculate);

        return charts;
    }
    
    public async Task<Dictionary<string, IEnumerable<int>>?> Get(Guid userId, IEnumerable<ExercisesTypeDto> exercisesTypeDto, ChartOption option, int period)
    {
        var userIdFromJwt = _userContextService.GetUserId;
        
        var exercisesType = _mapper.Map<IEnumerable<ExercisesTypeDto>, IEnumerable<ExercisesType>>(exercisesTypeDto);
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, userId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var calculate = GetCalculate(option);
        var charts = await _exerciseRepo.GetScores(exercisesType, userId, period, calculate);

        return charts;
    }

    private static Func<IEnumerable<Series>, int> GetCalculate(ChartOption option)
        => option switch
        {
            ChartOption.Score => (x) => x.Select(e => CalculateScore(e.Weight, e.NumberOfRepetitions)).Sum(),
            ChartOption.AverageRepetitions => (x) => (int)Math.Round(x.Average(e => e.NumberOfRepetitions)),
            ChartOption.AverageWeight => (x) => (int)Math.Round(x.Average(e => e.Weight)),
            ChartOption.MaxRep => (x) => x.Max(e => e.Weight),
            ChartOption.AverageSeriesCount => (x) => x.Count(),
            _ => throw new InvalidProgramException()
        };
    
    private static int CalculateScore(int weight, int reps)
        => (int)Math.Round(weight / (1.0278 - 0.0278 * reps), 2, MidpointRounding.AwayFromZero); //rpe
}