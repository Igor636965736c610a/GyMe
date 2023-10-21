using GyMeCore.Models.Entities;

namespace GyMeCore.IRepo;

public interface IExerciseRepo
{
    Task<Exercise?> Get(Guid exerciseId);
    Task<Exercise?> Get(Guid userId, string exercisesType);
    Task<IEnumerable<Exercise>> GetAll(IEnumerable<Guid> exerciseIds);
    Task<List<Exercise>> GetAll(Guid userId, int page, int size);
    Task<List<Exercise>> GetAll(Guid userId);
    Task<List<Exercise>> GetAll(Guid userId, IEnumerable<string> exercisesType);
    Task<Dictionary<Guid, Series?>> GetMaxReps(IEnumerable<Guid> exercisesId);
    Task<Series?> GetMaxRep(Guid exerciseId);
    Task<IEnumerable<int>> GetScore(Guid exerciseId, int period,
        Func<IEnumerable<Series>, int> calculate);
    Task<Dictionary<Guid, IEnumerable<int>>> GetScores(IEnumerable<Guid> exercisesId, int period,
        Func<IEnumerable<Series>, int> calculate);
    Task<Dictionary<string, IEnumerable<int>>> GetScores(IEnumerable<string> exercisesType, Guid userId, int period,
        Func<IEnumerable<Series>, int> calculate);
    Task Create(Exercise exercise);
    Task Update(Exercise exercise);
    Task Update(List<Exercise> exercises);
    Task Remove(Exercise exercise);
}