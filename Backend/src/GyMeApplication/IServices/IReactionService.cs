using GyMeApplication.Models.ReactionsAndComments;
using Microsoft.AspNetCore.Http;

namespace GyMeApplication.IServices;

public interface IReactionService
{
    Task<Guid> AddReaction(Guid simpleExerciseId, ReactionType reactionType);
    Task SetImageReaction(IFormFile image);
    Task<GetReactionDto> GetReaction(Guid reactionId);
    Task<IEnumerable<GetReactionDto>> GetReactions(Guid simpleExerciseId, int page, int size,
        ReactionType? reactionType);
    Task<IEnumerable<GetReactionCountDto>> GetSpecificReactionsCount(Guid simpleExerciseId);
    Task RemoveReaction(Guid reactionId);
}