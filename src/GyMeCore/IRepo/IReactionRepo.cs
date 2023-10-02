using GymAppCore.Models.Entities;
using GymAppCore.Models.Results;

namespace GymAppCore.IRepo;

public interface IReactionRepo
{
    Task Create(Reaction reaction);
    Task Update(Reaction reaction);
    Task<Reaction?> Get(Guid id);
    Task<Reaction?> Get(Guid simpleExerciseId, Guid userId);
    IQueryable<Reaction> GetAll(Guid simpleExerciseId, int page, int size);
    Task Remove(Reaction reaction);
    Task<IEnumerable<ReactionsCountResult>> GetSpecificReactionsCount(Guid simpleExerciseId);
    Task<int> GetReactionsCount(Guid simpleExerciseId);
    Task<Dictionary<Guid, int>> GetReactionsCount(IEnumerable<Guid> simpleExercisesId);
}