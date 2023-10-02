using GymAppInfrastructure.Models.ReactionsAndComments;
using GymAppInfrastructure.Models.ReactionsAndComments.BodyRequest;

namespace GymAppInfrastructure.IServices;

public interface ICommentService
{
    Task AddComment(PostCommentDto postCommentDto);
    Task<GetCommentDto> GetComment(Guid commentId);
    Task<IEnumerable<GetCommentDto>> GetCommentsSortedByPubTime(Guid simpleExerciseId, int page, int size);
    Task<IEnumerable<GetCommentDto>> GetCommentsSortedByReactionsCount(Guid simpleExerciseId, int page, int size);
    Task UpdateComment(PutCommentDto putCommentDto, Guid commentId);
    Task RemoveComment(Guid commentId);
}