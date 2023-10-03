using GyMeCore.Models.Entities;
using GyMeInfrastructure.Models.Exercise;

namespace GyMeInfrastructure.IServices;

public interface IExerciseService
{
    Task Create(PostExerciseDto postExerciseDto);
    Task Update(Guid exerciseId, PutExerciseDto putExerciseDto);
    Task Remove(Guid exerciseId);
    Task<GetExerciseDto> Get(Guid exerciseId);
    Task<IEnumerable<GetExerciseDto>> Get(Guid userId, int page, int size);
}