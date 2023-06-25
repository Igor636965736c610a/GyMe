using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.SimpleExercise;

namespace GymAppInfrastructure.IServices;

public interface ISimpleExerciseService
{
    Task Create(PostSimpleExerciseDto postSimpleExerciseDto, Guid userId);
    Task Update(Guid userId, Guid id, PutSimpleExerciseDto putExerciseDto);
    Task Remove(Guid userId, Guid id);
    Task<GetSimpleExerciseDto> Get(Guid userId, Guid id);
    Task<IEnumerable<GetSimpleExerciseDto>> Get(Guid jwtClaimId, Guid userId, Guid exerciseId, int page, int size);
}