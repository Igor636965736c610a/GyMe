using GymAppCore.Models.Entities;
using GymAppCore.Models.Results;

namespace GymAppCore.IRepo;

public interface ICommentReactionRepo
{
    IQueryable<CommentReaction> GetAll(Guid commentId);
    Task<IEnumerable<ReactionsCountResult>> GetSpecificCommentReactionsCount(Guid commentId);
    Task<int> GetCommentReactionsCount(Guid commentId);
    Task<Dictionary<Guid, int>> GetCommentReactionsCount(IEnumerable<Guid> commentsId);
}