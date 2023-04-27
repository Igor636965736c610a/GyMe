using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.SimpleExercise;

namespace GymAppInfrastructure.IServices;

public interface ISimpleExerciseService
{
    Task CreateSimpleExercise(PostSimpleExerciseDto postSimpleExerciseDto, Guid userId);
    Task UpdateSimpleExercise(Guid userId, Guid id, PutSimpleExerciseDto putExerciseDto);
    Task RemoveSimpleExercise(Guid userId, Guid id);
    Task<GetSimpleExerciseDto> GetSimpleExercise(Guid userId, Guid id);
    Task<IEnumerable<GetSimpleExerciseDto>> GetSimpleExercises(Guid userId, Guid exerciseId, int page, int size);
    Task<IEnumerable<GetSimpleExerciseDto>> GetForeignExercises(Guid jwtClaimId, Guid userId, Guid exerciseId, int page, int size);
}