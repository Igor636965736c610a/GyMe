using GyMeInfrastructure.Models.ReactionsAndComments;
using GyMeInfrastructure.Models.ReactionsAndComments.BodyRequest;

namespace GyMeInfrastructure.IServices;

public interface ICommentReactionService
{
    Task AddCommentReaction(PostCommentReactionDto postCommentReactionDto);
    Task<IEnumerable<GetCommentReactionDto>> GetCommentsReactions(Guid commentId, CommentReactionType? commentReactionType, int page, int size);
    Task<IEnumerable<GetCommentReactionCount>> GetSpecificCommentReactionCount(Guid commentId);
    Task RemoveCommentReaction(Guid commentReactionId);
}