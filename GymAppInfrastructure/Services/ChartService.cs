using System.Collections;
using AutoMapper;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Context;
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

    public ChartService(IExerciseRepo exerciseRepo, IUserRepo userRepo, IMapper mapper)
    {
        _exerciseRepo = exerciseRepo;
        _userRepo = userRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<int>?> Get(Guid jwtId, Guid exerciseId, ChartOption option, int period)
    {
        var exercise = await _exerciseRepo.Get(exerciseId);
        if (exercise is null)
            throw new InvalidOperationException();
        var owner = await _userRepo.Get(exercise.UserId);
        if(owner!.Id != jwtId && owner.PrivateAccount && await _userRepo.GetFriend(jwtId, owner.Id) is null)
            throw new ForbiddenException("You do not have the appropriate permissions");

        var calculate = GetCalculate(option);
        var chart = await _exerciseRepo.GetScore(exerciseId, period, calculate);

        return chart;
    }
    
    public async Task<IEnumerable<int>?> Get(Guid jwtId, Guid userUd, ExercisesTypeDto exercisesTypeDto, ChartOption option, int period)
    {
        var exerciseType = _mapper.Map<ExercisesTypeDto, ExercisesType>(exercisesTypeDto);
        var exercise = await _exerciseRepo.Get(userUd, exerciseType);
        if (exercise is null)
            throw new InvalidOperationException();
        var owner = await _userRepo.Get(exercise.UserId);
        if(owner!.Id != jwtId && owner.PrivateAccount && await _userRepo.GetFriend(jwtId, owner.Id) is null)
            throw new ForbiddenException("You do not have the appropriate permissions");

        var calculate = GetCalculate(option);
        var chart = await _exerciseRepo.GetScore(exercise.Id, period, calculate);

        return chart;
    }

    public async Task<Dictionary<Guid, IEnumerable<int>>?> Get(Guid jwtId, Guid userId, IEnumerable<Guid> ids, ChartOption option, int period)
    {
        var exercises = await _exerciseRepo.GetAll(ids);
        if(exercises.Any(x => x.UserId != userId))
            throw new ForbiddenException("You do not have the appropriate permissions");
        if (jwtId != userId)
        {
            var owner = await _userRepo.Get(userId);
            if(owner!.PrivateAccount && await _userRepo.GetFriend(jwtId, owner.Id) is null)
                throw new ForbiddenException("You do not have the appropriate permissions");
        }

        var calculate = GetCalculate(option);
        var charts = await _exerciseRepo.GetScores(ids, period, calculate);

        return charts;
    }
    
    public async Task<Dictionary<string, IEnumerable<int>>?> Get(Guid jwtId, Guid userId, IEnumerable<ExercisesTypeDto> exercisesTypeDto, ChartOption option, int period)
    {
        var exercisesType = _mapper.Map<IEnumerable<ExercisesTypeDto>, IEnumerable<ExercisesType>>(exercisesTypeDto);
        var exercises = await _exerciseRepo.GetAll(userId, exercisesType);
        if(exercises.Any(x => x.UserId != userId))
            throw new ForbiddenException("You do not have the appropriate permissions");
        if (jwtId != userId)
        {
            var owner = await _userRepo.Get(userId);
            if(owner!.PrivateAccount && await _userRepo.GetFriend(jwtId, owner.Id) is null)
                throw new ForbiddenException("You do not have the appropriate permissions");
        }

        var calculate = GetCalculate(option);
        var charts = await _exerciseRepo.GetScores(exercisesType, period, calculate);

        return charts;
    }

    private static Func<IEnumerable<Series>, int> GetCalculate(ChartOption option)
    {
        switch (option)
        {
            case ChartOption.Score:
            {
                return (x) => x.Select(e => CalculateScore(e.Weight, e.NumberOfRepetitions)).Sum();
            }
            case ChartOption.AverageRepetitions:
            {
                return (x) => (int)Math.Round(x.Average(e => e.NumberOfRepetitions));
            }
            case ChartOption.AverageWeight:
            {
                return (x) => (int)Math.Round(x.Average(e => e.Weight));
            }
            case ChartOption.MaxRep:
            {
                return (x) => x.Max(e => e.Weight);
            }
            case ChartOption.AverageSeriesCount:
            {
                return (x) => x.Count();
            }
            default:
                throw new InvalidProgramException();
        }
    }
    
    private static int CalculateScore(int weight, int reps)
        => (int)Math.Round(weight / (1.0278 - 0.0278 * reps), 2, MidpointRounding.AwayFromZero); //rpe
}