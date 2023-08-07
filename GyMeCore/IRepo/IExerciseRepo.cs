using GymAppCore.Models.Entities;

namespace GymAppCore.IRepo;

public interface IExerciseRepo 
{
    Task<Exercise?> Get(Guid exerciseId);
    Task<Exercise?> Get(Guid userId, ExercisesType exercisesType);
    Task<IEnumerable<Exercise>> GetAll(IEnumerable<Guid> exerciseIds);
    Task<List<Exercise>> GetAll(Guid userId, int page, int size);
    Task<List<Exercise>> GetAll(Guid userId);
    Task<List<Exercise>> GetAll(Guid userId, IEnumerable<ExercisesType> exercisesType);
    Task<Dictionary<Guid, Series>> GetMaxReps(IEnumerable<Guid> exercisesId);
    Task<Series?> GetMaxRep(Guid exerciseId);
    Task<IEnumerable<int>> GetScore(Guid exerciseId, int period,
        Func<IEnumerable<Series>, int> calculate);
    Task<Dictionary<Guid, IEnumerable<int>>> GetScores(IEnumerable<Guid> exercisesId, int period,
        Func<IEnumerable<Series>, int> calculate);
    Task<Dictionary<string, IEnumerable<int>>> GetScores(IEnumerable<ExercisesType> exercisesType, Guid userId, int period,
        Func<IEnumerable<Series>, int> calculate);
    Task<bool> Create(Exercise exercise);
    Task<bool> Update(Exercise exercise);
    Task<bool> Update(List<Exercise> exercises);
    Task<bool> Remove(Exercise exercise);
}