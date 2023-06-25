using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Exercise;

namespace GymAppInfrastructure.IServices;

public interface IExerciseService
{
    Task Create(PostExerciseDto postExerciseDto, Guid userId);
    Task Update(Guid userId, Guid exerciseId, PutExerciseDto putExerciseDto);
    Task Remove(Guid userId, Guid exerciseId);
    Task<GetExerciseDto> Get(Guid userId, Guid exerciseId);
    Task<IEnumerable<GetExerciseDto>> Get(Guid jwtClaimId, Guid userId, int page, int size);
}