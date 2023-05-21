using GymAppCore.Models.Entities;

namespace GymAppCore.IRepo;

public interface ISimpleExerciseRepo
{
    Task<SimpleExercise?> Get(Guid id);
    Task<List<SimpleExercise>> GetAll(Guid userId, Guid exerciseId, int page, int size);
    Task<List<SimpleExercise>> GetAll(Guid userId, Guid exerciseId);
    Task<Dictionary<Guid, SimpleExercise?>> GetMaxRepForExercises(IEnumerable<Guid> exercisesId);
    Task<SimpleExercise?> GetMaxRepForExercise(Guid exerciseId);

    Task<Dictionary<Exercise, int>> GetScoresss(IEnumerable<Guid> exercisesId, int size,
        Func<IEnumerable<Series>, int> calculate);

    Task<Dictionary<Exercise, int>> GetScoress(IEnumerable<Guid> exercisesId, int size,
        Func<IEnumerable<Series>, int> calculate);

    Task<bool> Create(SimpleExercise exercise);
    Task<bool> Update(SimpleExercise exercise);
    Task<bool> Remove(SimpleExercise exercise);
}