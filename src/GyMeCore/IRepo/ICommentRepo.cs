using GymAppCore.Models.Entities;

namespace GymAppCore.IRepo;

public interface ICommentRepo
{
    Task Create(Comment comment);
    Task<Comment?> Get(Guid id);
    Task<IEnumerable<Comment>> GetSortedByReactionsByCount(Guid simpleExerciseId, int page, int size);
    Task Update(Comment comment);
    Task Remove(Comment comment);
}