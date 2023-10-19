using System.Security.Policy;
using AutoMapper;
using GyMeApplication.Exceptions;
using GyMeApplication.IServices;
using GyMeApplication.Models.Series;
using GyMeApplication.Models.SimpleExercise;
using GyMeApplication.MyMapper;
using GyMeCore.IRepo;
using GyMeCore.Models.Entities;


namespace GyMeApplication.Services;

internal class SimpleExerciseService : ISimpleExerciseService
{
    private readonly ISimpleExerciseRepo _simpleExerciseRepo;
    private readonly IExerciseRepo _exerciseRepo;
    private readonly IUserRepo _userRepo;
    private readonly IUserContextService _userContextService;
    private readonly IReactionRepo _reactionRepo;
    private readonly ICommentRepo _commentRepo;
    private readonly IGyMeMapper _gyMeMapper;

    public SimpleExerciseService(ISimpleExerciseRepo simpleExerciseRepo, IExerciseRepo exerciseRepo, IUserRepo userRepo, IUserContextService userContextService,
        IReactionRepo reactionRepo, ICommentRepo commentRepo, IGyMeMapper gyMeMapper)
    {
        _simpleExerciseRepo = simpleExerciseRepo;
        _exerciseRepo = exerciseRepo;
        _userRepo = userRepo;
        _userContextService = userContextService;
        _reactionRepo = reactionRepo;
        _commentRepo = commentRepo;
        _gyMeMapper = gyMeMapper;
    }

    public async Task<Guid> Create(PostSimpleExerciseDto postSimpleExerciseDto)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var exercise = await _exerciseRepo.Get(postSimpleExerciseDto.ExerciseId);
        if (exercise is null)
            throw new NullReferenceException("Exercise does not exist");
        if (exercise.UserId != userIdFromJwt)
            throw new ForbiddenException("You do not have the appropriate permissions");
        if(!ValidSeries(postSimpleExerciseDto.PostSeriesDto))
            throw new InvalidOperationException("Invalid data");

        var series = postSimpleExerciseDto.PostSeriesDto.Select(x => new Series(x.NumberOfRepetitions, x.Weight)).ToList();
        
        var simpleExercise = new SimpleExercise(userIdFromJwt, exercise, series, postSimpleExerciseDto.Description);
        
        await _simpleExerciseRepo.Create(simpleExercise);
        return simpleExercise.Id;
    }

    public async Task Update(Guid id, PutSimpleExerciseDto putSimpleExerciseDto)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var simpleExercise = await _simpleExerciseRepo.Get(id);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");
        if (simpleExercise.UserId != userIdFromJwt)
            throw new ForbiddenException("You do not have access to this data");
        if(!ValidSeries(putSimpleExerciseDto.PutSeriesDto))
            throw new InvalidOperationException("Invalid data");
        
        var series = putSimpleExerciseDto.PutSeriesDto.Select(x => new Series(x.NumberOfRepetitions, x.Weight)).ToList();
        
        simpleExercise.Description = putSimpleExerciseDto.Description;
        simpleExercise.Series = series;

        await _simpleExerciseRepo.Update(simpleExercise);
    }

    public async Task Remove(Guid id)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var simpleExercise = await _simpleExerciseRepo.Get(id);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");
        if (simpleExercise.UserId != userIdFromJwt)
            throw new ForbiddenException("You do not have access to this data");

        await _simpleExerciseRepo.Remove(simpleExercise);
    }

    public async Task<GetSimpleExerciseDto> Get(Guid id)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var simpleExercise = await _simpleExerciseRepo.Get(id);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");
        
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, simpleExercise.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var reactionsCount = await _reactionRepo.GetReactionsCount(simpleExercise.Id);
        var commentsCount = await _commentRepo.GetCommentsCount(simpleExercise.Id);
        var simpleExerciseDto = _gyMeMapper.GetSimpleExerciseDtoMap(simpleExercise, reactionsCount, commentsCount);

        return simpleExerciseDto;
    }

    public async Task<IEnumerable<GetSimpleExerciseDto>> Get(Guid exerciseId, int page, int size)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var exercise = await _exerciseRepo.Get(exerciseId);
        if (exercise is null)
            throw new InvalidOperationException("Exercise does not exist");
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, exercise.UserId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var simpleExercises = await _simpleExerciseRepo.GetAll(exerciseId, page, size);
        var simpleExercisesId = simpleExercises.Select(x => x.Id);
        var exercisesId = simpleExercisesId as Guid[] ?? simpleExercisesId.ToArray();
        var reactionsCount = await _reactionRepo.GetReactionsCount(exercisesId);
        var commentsCount = await _commentRepo.GetCommentsCount(exercisesId);
        var simpleExercisesDto = simpleExercises.Select(x => _gyMeMapper.GetSimpleExerciseDtoMap(x, reactionsCount[x.Id], commentsCount[x.Id]));

        return simpleExercisesDto;
    }
    
    private static bool ValidSeries(IEnumerable<BaseSeriesDto> series)
    {
        var baseSeriesDto = series as BaseSeriesDto[] ?? series.ToArray();
        return baseSeriesDto.Length <= 30;
    }
}