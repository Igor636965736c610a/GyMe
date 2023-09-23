using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppCore.Models.Results;
using GymAppInfrastructure.Models.ReactionsAndComments.BodyRequest;
using GymAppInfrastructure.Options;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;

namespace GymAppInfrastructure.Repo;

public class ReactionRepo : IReactionRepo
{
    private readonly GyMePostgresContext _gyMePostgresContext;

    public ReactionRepo(GyMePostgresContext gyMePostgresContext)
    {
        _gyMePostgresContext = gyMePostgresContext;
    }

    public async Task Create(Reaction reaction)
    {
        await _gyMePostgresContext.Reactions.AddAsync(reaction);
        await UtilsRepo.SaveDatabaseChanges(_gyMePostgresContext);
    }

    public async Task Update(Reaction reaction)
    {
        _gyMePostgresContext.Reactions.Update(reaction); 
        await UtilsRepo.SaveDatabaseChanges(_gyMePostgresContext);
    }

    public async Task<Reaction?> Get(Guid id)
        => await _gyMePostgresContext.Reactions.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Reaction?> Get(Guid simpleExerciseId, Guid userId)
        => await _gyMePostgresContext.Reactions.FirstOrDefaultAsync(x =>
            x.SimpleExerciseId == simpleExerciseId && x.UserId == userId);

    public async Task<IQueryable<Reaction>> GetAll(Guid simpleExerciseId, int page, int size)
        => _gyMePostgresContext.Reactions
            .Where(x => x.SimpleExerciseId == simpleExerciseId);

    public async Task Remove(Reaction reaction)
    {
        _gyMePostgresContext.Reactions.Remove(reaction);
        await UtilsRepo.SaveDatabaseChanges(_gyMePostgresContext);
    }
    
    public async Task<IEnumerable<ReactionsCountResult>> GetConcreteReactionsCount(Guid simpleExerciseId)
        => await _gyMePostgresContext.Reactions
            .Where(x => x.SimpleExerciseId == simpleExerciseId)
            .GroupBy(x => x.ReactionType)
            .Select(x => new ReactionsCountResult()
            {
                ReactionType = x.Key,
                Emoji = x.First().Emoji,
                Count = x.Count()
            }).ToListAsync();

    public int GetReactionsCount(Guid simpleExerciseId)
        => _gyMePostgresContext.Reactions
            .Count(x => x.SimpleExerciseId == simpleExerciseId);
}