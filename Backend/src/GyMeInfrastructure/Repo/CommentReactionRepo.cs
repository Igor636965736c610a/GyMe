using GyMeCore.IRepo;
using GyMeCore.Models.Entities;
using GyMeCore.Models.Results;
using GyMeInfrastructure.Options;
using Microsoft.EntityFrameworkCore;

namespace GyMeInfrastructure.Repo;

public class CommentReactionRepo : ICommentReactionRepo
{
    private readonly GyMePostgresContext _gyMePostgresContext;

    public CommentReactionRepo(GyMePostgresContext gyMePostgresContext)
    {
        _gyMePostgresContext = gyMePostgresContext;
    }

    public async Task Create(CommentReaction commentReaction)
    {
        await _gyMePostgresContext.CommentReactions.AddAsync(commentReaction);
        await _gyMePostgresContext.SaveChangesAsync();
    }

    public async Task<CommentReaction?> Get(Guid commentReactionId)
        => await _gyMePostgresContext.CommentReactions.FirstOrDefaultAsync(x => x.Id == commentReactionId);
    
    public async Task<CommentReaction?> Get(Guid userId, Guid commentId)
        => await _gyMePostgresContext.CommentReactions.FirstOrDefaultAsync(x =>
            x.CommentId == commentId && x.UserId == userId);
    
    public IQueryable<CommentReaction> GetAll(Guid commentId)
        => _gyMePostgresContext.CommentReactions
            .Include(x => x.User)
            .Where(x => x.CommentId == commentId);
    
    public async Task<IEnumerable<CommentReactionsCount>> GetSpecificCommentReactionsCount(Guid commentId)
        => await _gyMePostgresContext.CommentReactions
            .Where(x => x.CommentId == commentId)
            .GroupBy(x => x.ReactionType)
            .Select(x => new CommentReactionsCount()
            {
                ReactionType = x.Key,
                Emoji = x.First().Emoji,
                Count = x.Count()
            }).ToListAsync();
    
    public async Task<int> GetCommentReactionsCount(Guid commentId)
        => await _gyMePostgresContext.CommentReactions
            .CountAsync(x => x.CommentId == commentId);

    public async Task<Dictionary<Guid, int>> GetCommentReactionsCount(IEnumerable<Guid> commentsId)
        => await _gyMePostgresContext.Comments
            .Where(x => commentsId.Contains(x.Id))
            .Include(x => x.CommentReactions)
            .ToDictionaryAsync(x => x.Id, x => x.CommentReactions.Count);

    public async Task Update(CommentReaction commentReaction)
    {
        _gyMePostgresContext.CommentReactions.Update(commentReaction);
        await _gyMePostgresContext.SaveChangesAsync();
    }

    public async Task Remove(CommentReaction commentReaction)
    {
        _gyMePostgresContext.CommentReactions.Remove(commentReaction);
        await _gyMePostgresContext.SaveChangesAsync();
    }
}