using GyMeCore.Models.Entities;
using GyMeCore.Models.Results;

namespace GyMeCore.IRepo;

public interface IReactionRepo
{
    Task Create(Reaction reaction);
    Task Update(Reaction reaction);
    Task<Reaction?> Get(Guid id);
    Task<Reaction?> Get(Guid simpleExerciseId, Guid userId);
    IQueryable<Reaction> GetAll(Guid simpleExerciseId);
    Task Remove(Reaction reaction);
    Task<IEnumerable<ReactionsCountResult>> GetSpecificReactionsCount(Guid simpleExerciseId);
    Task<int> GetReactionsCount(Guid simpleExerciseId);
    Task<Dictionary<Guid, int>> GetReactionsCount(IEnumerable<Guid> simpleExercisesId);
}