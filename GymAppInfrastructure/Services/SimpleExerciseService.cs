using AutoMapper;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.SimpleExercise;
using GymAppInfrastructure.Exceptions;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.MyMapper;


namespace GymAppInfrastructure.Services;

internal class SimpleExerciseService : ISimpleExerciseService
{
    private readonly ISimpleExerciseRepo _simpleExerciseRepo;
    private readonly IExerciseRepo _exerciseRepo;
    private readonly IUserRepo _userRepo;
    public SimpleExerciseService(ISimpleExerciseRepo simpleExerciseRepo, IExerciseRepo exerciseRepo, IUserRepo userRepo)
    {
        _simpleExerciseRepo = simpleExerciseRepo;
        _exerciseRepo = exerciseRepo;
        _userRepo = userRepo;
    }
    
    public async Task Create(PostSimpleExerciseDto postSimpleExerciseDto, Guid userId)
    {
        var exercise = await _exerciseRepo.Get(postSimpleExerciseDto.ExerciseId);
        if (exercise is null)
            throw new InvalidOperationException("Exercise does not exist");
        if(exercise.UserId != userId)
            throw new ForbiddenException("You do not have the appropriate permissions");
        
        var simpleExercise = new SimpleExercise(DateTime.UtcNow, postSimpleExerciseDto.Description, userId, exercise, postSimpleExerciseDto.Series);
        var series = UtilsServices.SeriesFromString(postSimpleExerciseDto.Series, simpleExercise);
        simpleExercise.Series = series;
        
        if (!await _simpleExerciseRepo.Create(simpleExercise))
            throw new SaveChangesDbException("something went wrong while saving database changes");
    }

    public async Task Update(Guid userId, Guid id, PutSimpleExerciseDto putExerciseDto)
    {
        var simpleExercise = await _simpleExerciseRepo.Get(id);
        if (simpleExercise is null)
            throw new InvalidOperationException("Not Found");
        if (simpleExercise.UserId != userId)
            throw new ForbiddenException("You do not have access to this data");
        var series = UtilsServices.SeriesFromString(putExerciseDto.Series, simpleExercise);
        simpleExercise.Description = putExerciseDto.Description;
        simpleExercise.SeriesString = putExerciseDto.Series;
        simpleExercise.Series = series;
        
        if(!await _simpleExerciseRepo.Update(simpleExercise))
            throw new SaveChangesDbException("something went wrong while saving database changes");
    }

    public async Task Remove(Guid userId, Guid id)
    {
        var simpleExercise = await _simpleExerciseRepo.Get(id);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");
        if (simpleExercise.UserId != userId)
            throw new ForbiddenException("You do not have access to this data");

        if(!await _simpleExerciseRepo.Remove(simpleExercise))
            throw new SaveChangesDbException("something went wrong while saving database changes");
    }

    public async Task<GetSimpleExerciseDto> Get(Guid userId, Guid id)
    {
        var simpleExercise = await _simpleExerciseRepo.Get(id);
        if (simpleExercise is null)
            throw new NullReferenceException("Not Found");
        var owner = await _userRepo.Get(simpleExercise.UserId);
        
        if (owner!.PrivateAccount && owner.Id != userId && await _userRepo.GetFriend(userId, userId) is null)
            throw new ForbiddenException("You do not have the appropriate permissions");

        var simpleExerciseDto = GetSimpleExerciseDtoMap.Map(simpleExercise);

        return simpleExerciseDto;
    }

    public async Task<IEnumerable<GetSimpleExerciseDto>> Get(Guid jwtClaimId, Guid userId, Guid exerciseId, int page, int size)
    {
        var owner = await _userRepo.Get(userId);
        if (owner is null)
            throw new InvalidOperationException("User does not exist");
        if (owner.PrivateAccount && userId != jwtClaimId && await _userRepo.GetFriend(userId, jwtClaimId) is null)
            throw new ForbiddenException("You do not have the appropriate permissions");

        var simpleExercises = await _simpleExerciseRepo.GetAll(userId, exerciseId, page, size);
        var simpleExercisesDto = simpleExercises.Select(x => GetSimpleExerciseDtoMap.Map(x));

        return simpleExercisesDto;
    }
}