using GymAppCore.Models.Entities;
using GymAppInfrastructure.Models.SimpleExercise;

namespace GymAppInfrastructure.IServices;

public interface ISimpleExerciseService
{
    Task Create(PostSimpleExerciseDto postSimpleExerciseDto);
    Task Update(Guid id, PutSimpleExerciseDto putExerciseDto);
    Task Remove(Guid id);
    Task<GetSimpleExerciseDto> Get(Guid id);
    Task<IEnumerable<GetSimpleExerciseDto>> Get(Guid exerciseId, int page, int size);
}