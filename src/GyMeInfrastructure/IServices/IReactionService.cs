using GymAppInfrastructure.Models.ReactionsAndComments;
using Microsoft.AspNetCore.Http;

namespace GymAppInfrastructure.IServices;

public interface IReactionService
{
    Task AddEmojiReaction(Guid simpleExerciseId, ReactionType reactionType);
    Task SetImageReaction(IFormFile image);
    Task<IEnumerable<GetReactionDto>> GetReactions(Guid simpleExerciseId, int page, int size,
        ReactionType? reactionType);
    Task<IEnumerable<GetReactionCountDto>> GetReactionsCount(Guid simpleExerciseId);
    Task RemoveReaction(Guid reactionId);
}