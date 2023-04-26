using GymAppCore.Models.Entities;

namespace GymAppCore.IRepo;

public interface IExerciseRepo
{
    Task<Exercise?> Get(Guid exerciseId);
    Task<Exercise?> Get(Guid userId, ExercisesType exercisesType);
    Task<List<Exercise>> GetAll(Guid userId, int page, int size);
    Task<List<Exercise>> GetAll(Guid userId);
    Task<bool> Create(Exercise exercise);
    Task<bool> Update(Exercise exercise);
    Task<bool> Update(List<Exercise> exercises);
    Task<bool> Remove(Exercise exercise);
}