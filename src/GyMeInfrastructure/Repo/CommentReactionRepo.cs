using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Options;

namespace GymAppInfrastructure.Repo;

public class CommentReactionRepo : ICommentReactionRepo
{
    private readonly GyMePostgresContext _gyMePostgresContext;

    public CommentReactionRepo(GyMePostgresContext gyMePostgresContext)
    {
        _gyMePostgresContext = gyMePostgresContext;
    }

    public async Task<IQueryable<CommentReaction>> GetAll(Guid commentId, int page, int size)
    {
        return await Task.FromResult(_gyMePostgresContext.CommentReactions
            .Where(x => x.CommentId == commentId)
            .OrderBy(x => x.TimeStamp)
            .Skip(page * size)
            .Take(size));
    }
}