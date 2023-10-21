using GyMeApplication.Models.SimpleExercise;
using GyMeCore.Models.Entities;

namespace GyMeApplication.IServices;

public interface ISimpleExerciseService
{
    Task<Guid> Create(PostSimpleExerciseDto postSimpleExerciseDto);
    Task<GetSimpleExerciseDto> Update(Guid id, PutSimpleExerciseDto putSimpleExerciseDto);
    Task Remove(Guid id);
    Task<GetSimpleExerciseDto> Get(Guid id);
    Task<IEnumerable<GetSimpleExerciseDto>> Get(Guid exerciseId, int page, int size);
}