using GymAppCore.Models.Entities;

namespace GymAppCore.IRepo;

public interface ICommentReactionRepo
{
    Task<IQueryable<CommentReaction>> Test();
}