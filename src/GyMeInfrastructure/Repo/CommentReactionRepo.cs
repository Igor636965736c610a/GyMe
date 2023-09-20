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

    public async Task<IQueryable<CommentReaction>> Test()
    {
        return _gyMePostgresContext.CommentReactions.Where(x => x.Emoji == "test");
    }
}