using GyMeApplication.Models.Exercise;
using GyMeCore.Models.Entities;

namespace GyMeApplication.IServices;

public interface IExerciseService
{
    Task<Guid> Create(PostExerciseDto postExerciseDto);
    Task Update(Guid exerciseId, PutExerciseDto putExerciseDto);
    Task Remove(Guid exerciseId);
    Task<GetExerciseDto> Get(Guid exerciseId);
    Task<IEnumerable<GetExerciseDto>> Get(Guid userId, int page, int size);
}