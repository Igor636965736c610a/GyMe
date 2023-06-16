using System.Net.NetworkInformation;
using AutoMapper;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Exercise;
using GymAppInfrastructure.Dtos.Series;
using GymAppInfrastructure.Exceptions;
using GymAppInfrastructure.IServices;

namespace GymAppInfrastructure.Services;

public class ExerciseService : IExerciseService
{
    private readonly IMapper _mapper;
    private readonly IExerciseRepo _exerciseRepo;
    private readonly IUserRepo _userRepo;
    public ExerciseService(IMapper autoMapper, IExerciseRepo exerciseRepo, IUserRepo userRepo)
    {
        _mapper = autoMapper;
        _exerciseRepo = exerciseRepo;
        _userRepo = userRepo;
    }
    
    public async Task CreateExercise(PostExerciseDto postExerciseDto, Guid userId)
    {
        if (postExerciseDto.Position is null)
            postExerciseDto.Position = 0;
        
        var existingExercise = await _exerciseRepo.Get(userId, postExerciseDto.ExercisesType);
        if (existingExercise is not null)
        {
            throw new InvalidOperationException("Exercise already exist");
        }
        
        var exercises = await _exerciseRepo.GetAll(userId);
        var exercise = new Exercise(postExerciseDto.ExercisesType, postExerciseDto.Position.Value, userId);

        var toUpdate = AddExercise(exercise, exercises);
        
        if(!await _exerciseRepo.Create(exercise) || !await _exerciseRepo.Update(toUpdate))
            throw new SaveChangesDbException("something went wrong while saving database changes");
    }

    public async Task UpdateExercise(Guid userId, Guid exerciseId, PutExerciseDto putExerciseDto)
    {
        var exercise = await _exerciseRepo.Get(exerciseId);
        if (exercise is null)
            throw new InvalidOperationException("Not Found");
        if(exercise.UserId != userId)
            throw new ForbiddenException("You do not have the appropriate permissions");
        
        var exercises = await _exerciseRepo.GetAll(userId);
        exercises.Remove(exercise);
        exercise.Position = putExerciseDto.Position;
        AddExercise(exercise, exercises);
        
        if(!await _exerciseRepo.Update(exercise) || !await _exerciseRepo.Update(exercises))
            throw new SaveChangesDbException("something went wrong while saving database changes");
    }

    public async Task RemoveExercise(Guid userId, Guid exerciseId)
    {
        var exercises = await _exerciseRepo.GetAll(userId);
        var exercise = await _exerciseRepo.Get(exerciseId);
        if (exercise is null)
            throw new InvalidOperationException("Not Found");
        if (exercise.UserId != userId)
            throw new ForbiddenException("You do not have the appropriate permissions");
        
        var toUpdate = RemoveExercise(exercise, exercises);
        
        if(!await _exerciseRepo.Remove(exercise) || !await _exerciseRepo.Update(toUpdate))
            throw new SaveChangesDbException("something went wrong while saving database changes");
    }

    public async Task<GetExerciseDto> GetExercise(Guid userId, Guid exerciseId)
    {
        var exercise = await _exerciseRepo.Get(exerciseId);
        if (exercise is null)
            throw new NullReferenceException("Not Found");
        var owner = await _userRepo.Get(exercise.UserId);
        
        if (owner!.PrivateAccount && owner.Id != userId && await _userRepo.GetFriend(userId, userId) is null)
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

    public async Task<IEnumerable<GetExerciseDto>> GetExercises(Guid jwtClaimId, Guid userId, int page, int size)
    {
        var owner = await _userRepo.Get(userId);
        if (owner is null)
            throw new InvalidOperationException("User does not exist");
        if (owner.PrivateAccount && userId != jwtClaimId && await _userRepo.GetFriend(userId, jwtClaimId) is null)
            throw new ForbiddenException("You do not have the appropriate permissions");

        var exercises = await _exerciseRepo.GetAll(userId, page, size);

        return await UtilGetExercises(exercises);
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

    private async Task<IEnumerable<GetExerciseDto>> UtilGetExercises(List<Exercise> exercises)
    {
        var ids = exercises.Select(x => x.Id);
        var maxReps = await _exerciseRepo.GetMaxReps(ids);
        var maxRepsDto = maxReps.ToDictionary(x => x.Key, x => _mapper.Map<Series, GetSeriesDto>(x.Value));
        var exercisesDto = exercises.Select(x => new GetExerciseDto()
        {
            Id = x.Id,
            ExercisesType = x.ExercisesType,
            MaxRep = maxRepsDto[x.Id]
        });

        return exercisesDto;
    }
}