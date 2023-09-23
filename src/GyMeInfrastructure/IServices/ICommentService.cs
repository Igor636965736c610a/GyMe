using GymAppInfrastructure.Models.ReactionsAndComments.BodyRequest;

namespace GymAppInfrastructure.IServices;

public interface ICommentService
{
    Task AddComment(PostCommentDto postCommentDto);
    Task Get(Guid commentId);
}