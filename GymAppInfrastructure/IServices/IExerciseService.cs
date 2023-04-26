using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Exercise;

namespace GymAppInfrastructure.IServices;

public interface IExerciseService
{
    Task CreateExercise(PostExerciseDto postExerciseDto, Guid userId);
    Task UpdateExercise(Guid userId, Guid exerciseId, PutExerciseDto putExerciseDto);
    Task RemoveExercise(Guid userId, Guid exerciseId);
    Task<GetExerciseDto> GetExercise(Guid userId, Guid exerciseId);
    Task<IEnumerable<GetExerciseDto>> GetExercises(Guid userId, int page, int size);
    Task<IEnumerable<GetExerciseDto>> GetForeignExercises(Guid jwtClaimId, Guid userId, int page, int size);
}