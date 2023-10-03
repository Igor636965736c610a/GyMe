using GyMeCore.Models.Entities;

namespace GyMeCore.IRepo;

public interface ICommentRepo
{
    Task Create(Comment comment);
    Task<Comment?> Get(Guid id);
    Task<List<Comment>> GetSortedByReactionsCount(Guid simpleExerciseId, int page, int size);
    Task<List<Comment>> GetSortedByTimeStamp(Guid simpleExerciseId, int page, int size);
    Task Update(Comment comment);
    Task Remove(Comment comment);
    Task<int> GetCommentsCount(Guid simpleExerciseId);
    Task<Dictionary<Guid, int>> GetCommentsCount(IEnumerable<Guid> simpleExercisesId);
}