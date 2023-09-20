using GymAppInfrastructure.Models.ReactionsAndComments;

namespace GymAppInfrastructure.IServices;

public interface ICommentService
{
    Task AddComment(PostCommentDto postCommentDto);
    Task Get(Guid commentId);
}