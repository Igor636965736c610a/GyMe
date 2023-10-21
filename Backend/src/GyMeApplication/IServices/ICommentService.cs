using GyMeApplication.Models.ReactionsAndComments;
using GyMeApplication.Models.ReactionsAndComments.BodyRequest;

namespace GyMeApplication.IServices;

public interface ICommentService
{
    Task<Guid> AddComment(PostCommentDto postCommentDto);
    Task<GetCommentDto> GetComment(Guid commentId);
    Task<IEnumerable<GetCommentDto>> GetCommentsSortedByPubTime(Guid simpleExerciseId, int page, int size);
    Task<IEnumerable<GetCommentDto>> GetCommentsSortedByReactionsCount(Guid simpleExerciseId, int page, int size);
    Task<GetCommentDto> UpdateComment(PutCommentDto putCommentDto, Guid commentId);
    Task RemoveComment(Guid commentId);
}