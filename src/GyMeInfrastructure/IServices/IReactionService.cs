using GymAppInfrastructure.Models.ReactionsAndComments;
using Microsoft.AspNetCore.Http;

namespace GymAppInfrastructure.IServices;

public interface IReactionService
{
    Task AddEmojiReaction(Guid simpleExerciseId, Emoji emoji);
    Task AddImageReaction(Guid simpleExerciseId, IFormFile image);
    Task<IEnumerable<GetReactionDto>> GetReactions(Guid simpleExerciseId, int page, int size,
        ReactionType? reactionType);
    Task RemoveReaction(Guid reactionId);
}