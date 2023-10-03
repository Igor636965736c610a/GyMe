using GyMeCore.Models.Entities;
using GyMeInfrastructure.Models.SimpleExercise;

namespace GyMeInfrastructure.IServices;

public interface ISimpleExerciseService
{
    Task Create(PostSimpleExerciseDto postSimpleExerciseDto);
    Task Update(Guid id, PutSimpleExerciseDto putExerciseDto);
    Task Remove(Guid id);
    Task<GetSimpleExerciseDto> Get(Guid id);
    Task<IEnumerable<GetSimpleExerciseDto>> Get(Guid exerciseId, int page, int size);
}