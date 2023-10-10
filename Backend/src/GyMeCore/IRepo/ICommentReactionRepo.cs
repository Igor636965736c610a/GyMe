using GyMeCore.Models.Entities;
using GyMeCore.Models.Results;

namespace GyMeCore.IRepo;

public interface ICommentReactionRepo
{
    Task Create(CommentReaction commentReaction);
    Task<CommentReaction?> Get(Guid commentReactionId);
    Task<CommentReaction?> Get(Guid userId, Guid commentId);
    IQueryable<CommentReaction> GetAll(Guid commentId);
    Task<IEnumerable<CommentReactionsCount>> GetSpecificCommentReactionsCount(Guid commentId);
    Task<int> GetCommentReactionsCount(Guid commentId);
    Task<Dictionary<Guid, int>> GetCommentReactionsCount(IEnumerable<Guid> commentsId);
    Task Update(CommentReaction commentReaction);
    Task Remove(CommentReaction commentReaction);
}