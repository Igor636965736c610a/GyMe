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
        => await _gyMePostgresContext.Comments.FirstOrDefaultAsync(x => x.Id == id);
    
    public async Task<IEnumerable<Comment>> GetSortedByReactionsByCount(Guid simpleExerciseId, int page, int size)
       => await _gyMePostgresContext.Comments
           .Where(x => x.SimpleExerciseId == simpleExerciseId)
           .OrderBy(x => x.CommentReactions)
           .Skip(page * size)
           .Take(size)
           .ToListAsync();
    
    public async Task<IEnumerable<Comment>> GetSortedByTimeStamp(Guid simpleExerciseId, int page, int size)
        => await _gyMePostgresContext.Comments
            .Where(x => x.SimpleExerciseId == simpleExerciseId)
            .OrderBy(x => x.TimeStamp)
            .Skip(page * size)
            .Take(size)
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
}