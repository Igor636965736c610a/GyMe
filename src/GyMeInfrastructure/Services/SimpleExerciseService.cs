using AutoMapper;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Models.Series;
using GymAppInfrastructure.Models.SimpleExercise;
using GymAppInfrastructure.Exceptions;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.MyMapper;


namespace GymAppInfrastructure.Services;

internal class SimpleExerciseService : ISimpleExerciseService
{
    private readonly ISimpleExerciseRepo _simpleExerciseRepo;
    private readonly IExerciseRepo _exerciseRepo;
    private readonly IUserRepo _userRepo;
    private readonly IMapper _mapper;
    private readonly IUserContextService _userContextService;

    public SimpleExerciseService(ISimpleExerciseRepo simpleExerciseRepo, IExerciseRepo exerciseRepo, IUserRepo userRepo, IMapper mapper, IUserContextService userContextService)
    {
        _simpleExerciseRepo = simpleExerciseRepo;
        _exerciseRepo = exerciseRepo;
        _userRepo = userRepo;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public async Task Create(PostSimpleExerciseDto postSimpleExerciseDto)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var exercise = await _exerciseRepo.Get(postSimpleExerciseDto.ExerciseId);
        if (exercise is null)
            throw new InvalidOperationException("Exercise does not exist");
        if (exercise.UserId != userIdFromJwt)
            throw new ForbiddenException("You do not have the appropriate permissions");
        if(!ValidSeries(postSimpleExerciseDto.SeriesDto))
            throw new InvalidOperationException("Invalid data");

        var series = _mapper.Map<IEnumerable<BaseSeriesDto>, IEnumerable<Series>>(postSimpleExerciseDto.SeriesDto).ToList();

        var simpleExercise = new SimpleExercise(DateTime.UtcNow, postSimpleExerciseDto.Description, userIdFromJwt, exercise, series);

        await _simpleExerciseRepo.Create(simpleExercise);
    }

    public async Task Update(Guid id, PutSimpleExerciseDto putExerciseDto)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var simpleExercise = await _simpleExerciseRepo.Get(id);
        if (simpleExercise is null)
            throw new InvalidOperationException("Not Found");
        if (simpleExercise.UserId != userIdFromJwt)
            throw new ForbiddenException("You do not have access to this data");
        if(!ValidSeries(putExerciseDto.SeriesDto))
            throw new InvalidOperationException("Invalid data");
        
        var series = _mapper.Map<IEnumerable<BaseSeriesDto>, IEnumerable<Series>>(putExerciseDto.SeriesDto).ToList();
        
        simpleExercise.Description = putExerciseDto.Description;
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

        var simpleExerciseDto = GetSimpleExerciseDtoMap.Map(simpleExercise);

        return simpleExerciseDto;
    }

    public async Task<IEnumerable<GetSimpleExerciseDto>> Get(Guid userId, Guid exerciseId, int page, int size)
    {
        var userIdFromJwt = _userContextService.UserId;
        
        var owner = await _userRepo.GetOnlyValid(userId);
        if (owner is null)
            throw new InvalidOperationException("User does not exist");
        if(!await UtilsServices.CheckResourceAccessPermissions(userIdFromJwt, userId, _userRepo))
            throw new ForbiddenException("You do not have the appropriate permissions");

        var simpleExercises = await _simpleExerciseRepo.GetAll(userId, exerciseId, page, size);
        var simpleExercisesDto = simpleExercises.Select(x => GetSimpleExerciseDtoMap.Map(x));

        return simpleExercisesDto;
    }
    
    private static bool ValidSeries(IEnumerable<BaseSeriesDto> series)
    {
        var baseSeriesDtos = series as BaseSeriesDto[] ?? series.ToArray();
        if (baseSeriesDtos.Length > 30)
            return false;
        return !baseSeriesDtos.Any(x => x.NumberOfRepetitions > 999 || x.Weight > 99999);
    }
}