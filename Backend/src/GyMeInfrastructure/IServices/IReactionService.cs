using GyMeInfrastructure.Models.ReactionsAndComments;
using Microsoft.AspNetCore.Http;

namespace GyMeInfrastructure.IServices;

public interface IReactionService
{
    Task AddEmojiReaction(Guid simpleExerciseId, ReactionType reactionType);
    Task SetImageReaction(IFormFile image);
    Task<IEnumerable<GetReactionDto>> GetReactions(Guid simpleExerciseId, int page, int size,
        ReactionType? reactionType);
    Task<IEnumerable<GetReactionCountDto>> GetSpecificReactionsCount(Guid simpleExerciseId);
    Task RemoveReaction(Guid reactionId);
}