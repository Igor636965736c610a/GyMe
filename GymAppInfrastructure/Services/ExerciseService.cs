using AutoMapper;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Exercise;
using GymAppInfrastructure.Exceptions;
using GymAppInfrastructure.IServices;

namespace GymAppInfrastructure.Services;

public class ExerciseService : IExerciseService
{
    private readonly IMapper _mapper;
    private readonly IExerciseRepo _exerciseRepo;
    public ExerciseService(IMapper autoMapper, IExerciseRepo exerciseRepo)
    {
        _mapper = autoMapper;
        _exerciseRepo = exerciseRepo;
    }
    
    public async Task CreateExercise(PostExerciseDto postExerciseDto, Guid userId)
    {
        if (postExerciseDto.Position is null)
            postExerciseDto.Position = 0;
        
        var existingExercise = await _exerciseRepo.Get(postExerciseDto.ExercisesType, userId);
        if (existingExercise is not null)
        {
            throw new InvalidOperationException("Exercise already exist");
        }
        
        var exercises = await _exerciseRepo.Get(userId);
        var exercise = new Exercise(postExerciseDto.ExercisesType, postExerciseDto.Position.Value, userId);

        var toUpdate = AddExercise(exercise, exercises);
        
        if(!await _exerciseRepo.Create(exercise) || !await _exerciseRepo.Update(toUpdate))
            throw new SaveChangesDbException("something went wrong while saving database changes");
    }

    public async Task UpdateExercise(Guid userId, ExercisesType exercisesType, PutExerciseDto putExerciseDto)
    {
        var exercise = await _exerciseRepo.Get(exercisesType, userId);
        if (exercise is null)
            throw new InvalidOperationException("Not Found");
        var exercises = await _exerciseRepo.Get(userId);
        exercises.Remove(exercise);
        exercise.Position = putExerciseDto.Position;
        AddExercise(exercise, exercises);
        
        if(!await _exerciseRepo.Update(exercise) || !await _exerciseRepo.Update(exercises))
            throw new SaveChangesDbException("something went wrong while saving database changes");
    }

    public async Task RemoveExercise(Guid userId, ExercisesType exercisesType)
    {
        var exercises = await _exerciseRepo.Get(userId);
        
        var exercise = await _exerciseRepo.Get(exercisesType, userId);
        if (exercise is null)
            throw new InvalidOperationException("Not Found");
        var toUpdate = RemoveExercise(exercise, exercises);
        
        if(!await _exerciseRepo.Remove(exercise) || !await _exerciseRepo.Update(toUpdate))
            throw new SaveChangesDbException("something went wrong while saving database changes");
    }

    public async Task<GetExerciseDto> GetExercise(Guid userId, ExercisesType exercisesType)
    {
        var exercise = await _exerciseRepo.Get(exercisesType, userId);
        if (exercise is null)
            throw new NullReferenceException("Not Found");
        var exerciseDto = _mapper.Map<Exercise, GetExerciseDto>(exercise);

        return exerciseDto;
    }

    public async Task<IEnumerable<GetExerciseDto>> GetExercises(Guid userId)
    {
        var exercises = await _exerciseRepo.Get(userId);
        var exercisesDto = _mapper.Map<IEnumerable<Exercise>, IEnumerable<GetExerciseDto>>(exercises);

        return exercisesDto;
    }

    private static List<Exercise> AddExercise(Exercise exercise, List<Exercise> exercises)
    {
        List<Exercise> output = new();
        if (exercises.Count == 0)
            return output;
        if (exercise.Position < 0)
        {
            exercise.Position = 0;
        }

        if (exercise.Position > exercises.Count - 1)
        {
            exercise.Position = exercises.Count;
            return output;
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
            return output;
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