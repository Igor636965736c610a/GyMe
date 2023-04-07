using AutoMapper;
using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Exercise;
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
    
    public async Task CreateExercise(PostExerciseDto postExerciseDto, User user)
    {
        var maxPosition = GetLastExercisePosition(user.Exercises);
        if (postExerciseDto.Position < 0)
        {
            postExerciseDto.Position = 0;
        }

        if (postExerciseDto.Position > maxPosition + 1)
        {
            postExerciseDto.Position = maxPosition + 1;
        }
        
        var hideExercise = await _exerciseRepo.Get(user, postExerciseDto.ExercisesType);
        if (hideExercise is not null)
        {
            hideExercise.Shown = true;
            AdjustTheQueueToNewPosition(user.Exercises, postExerciseDto.Position, maxPosition);
            hideExercise.Position = postExerciseDto.Position;
            await _exerciseRepo.Update(hideExercise);
            return;
        }
        var exerciseToAdd = new Exercise(postExerciseDto.ExercisesType, postExerciseDto.Position, user);
        AdjustTheQueueToNewPosition(user.Exercises, postExerciseDto.Position, maxPosition);
        await _exerciseRepo.Create(exerciseToAdd);
    }

    public async Task UpdateExercise(User user, ExercisesType exercisesType, PutExerciseDto putExerciseDto)
    {
        var exercise = await _exerciseRepo.Get(user, exercisesType);
        if (exercise is null)
            throw new NullReferenceException("Not Found");
        exercise.Position = putExerciseDto.Position;
        await _exerciseRepo.Update(exercise);
    }

    public async Task HideExercise(User user, ExercisesType exercisesType)
    {
        var exercise = await _exerciseRepo.Get(user, exercisesType);
        if (exercise is null)
            throw new NullReferenceException("Not Found");
        exercise.Shown = false;
        await _exerciseRepo.Update(exercise);
    }

    public async Task<GetExerciseDto> GetExercise(User user, ExercisesType exercisesType)
    {
        var exercise = await _exerciseRepo.Get(user, exercisesType);
        if (exercise is null)
            throw new NullReferenceException("Not Found");
        var exerciseDto = _mapper.Map<Exercise, GetExerciseDto>(exercise);

        return exerciseDto;
    }

    public async Task<IEnumerable<GetExerciseDto>> GetExercises(User user)
    {
        var exercises = await _exerciseRepo.Get(user);
        var exercisesDto = _mapper.Map<IEnumerable<Exercise>, IEnumerable<GetExerciseDto>>(exercises);

        return exercisesDto;
    }

    private int GetLastExercisePosition(IEnumerable<Exercise> exercises)
        => exercises.Max(x => x.Position);

    private void AdjustTheQueueToNewPosition(IEnumerable<Exercise> exercises, int oldPosition, int newPosition)
    {
        if (newPosition < oldPosition)
        {
            var elements = exercises.Where(x => x.Position > newPosition && x.Position <= oldPosition);
            foreach (var p in elements)
            {
                p.Position++;
                _exerciseRepo.Update(p);
            }
        }
        else
        {
            var elements = exercises.Where(x => x.Position < newPosition && x.Position >= oldPosition);
            foreach (var p in elements)
            {
                p.Position--;
                _exerciseRepo.Update(p);
            }
        }
    }
}