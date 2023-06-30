using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Exercise;

namespace GymAppInfrastructure.IServices;

public interface IExerciseService
{
    Task Create(PostExerciseDto postExerciseDto, Guid jwtId);
    Task Update(Guid jwtId, Guid exerciseId, PutExerciseDto putExerciseDto);
    Task Remove(Guid jwtId, Guid exerciseId);
    Task<GetExerciseDto> Get(Guid userId, Guid exerciseId);
    Task<IEnumerable<GetExerciseDto>> Get(Guid jwtClaimId, Guid userId, int page, int size);
}