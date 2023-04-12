using GymAppCore.Models.Entities;

namespace GymAppCore.IRepo;

public interface IExerciseRepo
{
    Task<Exercise?> Get(ExercisesType exercisesType, Guid userId);
    Task<List<Exercise>> Get(Guid userId);
    Task<bool> Create(Exercise exercise);
    Task<bool> Update(Exercise exercise);
    Task<bool> Update(List<Exercise> exercises);
    Task<bool> Remove(Exercise exercise);
}