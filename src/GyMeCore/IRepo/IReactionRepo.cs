using GymAppCore.Models.Entities;

namespace GymAppCore.IRepo;

public interface IReactionRepo
{
    Task Create(Reaction reaction);
    Task Update(Reaction reaction);
    Task<Reaction?> Get(Guid id);
    Task<Reaction?> Get(Guid simpleExerciseId, Guid userId);
    Task<IQueryable<Reaction>> GetAll(Guid simpleExerciseId, int page, int size);
    Task Remove(Reaction reaction);
}