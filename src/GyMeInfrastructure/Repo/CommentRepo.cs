using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Options;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;

namespace GymAppInfrastructure.Repo;

public class CommentRepo : ICommentRepo
{
    private readonly GyMePostgresContext _gyMePostgresContext;

    public CommentRepo(GyMePostgresContext gyMePostgresContext)
    {
        _gyMePostgresContext = gyMePostgresContext;
    }

    public async Task Create(Comment comment)
    {
        await _gyMePostgresContext.Comments.AddAsync(comment);
        await UtilsRepo.SaveDatabaseChanges(_gyMePostgresContext);
    }

    public async Task<Comment?> Get(Guid id)
        => await _gyMePostgresContext.Comments
            .Include(x => x.User)
            .Include(x => x.CommentReactions
                .OrderBy(z => z.TimeStamp)
                .Take(3))
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);
    
    public async Task<List<Comment>> GetSortedByReactionsCount(Guid simpleExerciseId, int page, int size)
       => await _gyMePostgresContext.Comments
           .Where(x => x.SimpleExerciseId == simpleExerciseId)
           .OrderBy(x => x.CommentReactions.Count)
           .Skip(page * size)
           .Take(size)
           .Include(x => x.User)
           .Include(x => x.CommentReactions
               .OrderBy(z => z.TimeStamp)
               .Take(3))
           .ThenInclude(x => x.User)
           .ToListAsync();
    
    public async Task<List<Comment>> GetSortedByTimeStamp(Guid simpleExerciseId, int page, int size)
        => await _gyMePostgresContext.Comments
            .Where(x => x.SimpleExerciseId == simpleExerciseId)
            .OrderBy(x => x.TimeStamp)
            .Skip(page * size)
            .Take(size)
            .Include(x => x.User)
            .Include(x => x.CommentReactions
                .OrderBy(z => z.TimeStamp)
                .Take(3))
            .ThenInclude(x => x.User)
            .ToListAsync();

    public async Task Update(Comment comment)
    {
        _gyMePostgresContext.Comments.Update(comment);
        await UtilsRepo.SaveDatabaseChanges(_gyMePostgresContext);
    }

    public async Task Remove(Comment comment)
    {
        _gyMePostgresContext.Comments.Remove(comment);
        await UtilsRepo.SaveDatabaseChanges(_gyMePostgresContext);
    }
    
    public async Task<int> GetCommentsCount(Guid simpleExerciseId)
        => await _gyMePostgresContext.Comments
            .CountAsync(x => x.SimpleExerciseId == simpleExerciseId);

    public async Task<Dictionary<Guid, int>> GetCommentsCount(IEnumerable<Guid> simpleExercisesId)
        => await _gyMePostgresContext.SimpleExercises
            .Where(x => simpleExercisesId.Contains(x.Id))
            .Include(x => x.Comments)
            .ToDictionaryAsync(x => x.Id, x => x.Comments.Count);
    
    // public async Task<Dictionary<Guid, int>> GetCommentsCount(IEnumerable<Guid> simpleExercisesId)
    //     => await _gyMePostgresContext.Comments
    //         .Where(x => simpleExercisesId.Contains(x.SimpleExerciseId))
    //         .GroupBy(x => x.SimpleExerciseId)
    //         .ToDictionaryAsync(x => x.Key, x => x.Count());
}