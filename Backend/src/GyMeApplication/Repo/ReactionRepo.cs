using GyMeApplication.Options;
using GyMeCore.IRepo;
using GyMeCore.Models.Entities;
using GyMeCore.Models.Results;
using GyMeApplication.Models.ReactionsAndComments.BodyRequest;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;

namespace GyMeApplication.Repo;

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
        await _gyMePostgresContext.SaveChangesAsync();
    }

    public async Task Update(Reaction reaction)
    {
        _gyMePostgresContext.Reactions.Update(reaction); 
        await _gyMePostgresContext.SaveChangesAsync();
    }

    public async Task<Reaction?> Get(Guid id)
        => await _gyMePostgresContext.Reactions.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Reaction?> Get(Guid simpleExerciseId, Guid userId)
        => await _gyMePostgresContext.Reactions.Include(x => x.User).FirstOrDefaultAsync(x =>
            x.SimpleExerciseId == simpleExerciseId && x.UserId == userId);

    public IQueryable<Reaction> GetAll(Guid simpleExerciseId)
        => _gyMePostgresContext.Reactions
            .Include(x => x.User)
            .Where(x => x.SimpleExerciseId == simpleExerciseId);

    public async Task Remove(Reaction reaction)
    {
        _gyMePostgresContext.Reactions.Remove(reaction);
        await _gyMePostgresContext.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<ReactionsCountResult>> GetSpecificReactionsCount(Guid simpleExerciseId)
        => await _gyMePostgresContext.Reactions
            .Where(x => x.SimpleExerciseId == simpleExerciseId)
            .GroupBy(x => x.ReactionType)
            .Select(x => new ReactionsCountResult()
            {
                ReactionType = x.Key,
                Emoji = x.First().Emoji,
                Count = x.Count()
            }).ToListAsync();

    public async Task<int> GetReactionsCount(Guid simpleExerciseId)
        => await _gyMePostgresContext.Reactions
            .CountAsync(x => x.SimpleExerciseId == simpleExerciseId);

    public async Task<Dictionary<Guid, int>> GetReactionsCount(IEnumerable<Guid> simpleExercisesId)
        => await _gyMePostgresContext.SimpleExercises
            .Where(x => simpleExercisesId.Contains(x.Id))
            .Include(x => x.Reactions)
            .ToDictionaryAsync(x => x.Id, x => x.Reactions.Count);
}