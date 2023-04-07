using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.SimpleExercise;

namespace GymAppInfrastructure.IServices;

public interface ISimpleExerciseService
{
    Task CreateSimpleExercise(PostSimpleExerciseDto postSimpleExerciseDto, User user);
    Task UpdateSimpleExercise(User user, Guid id, PutSimpleExerciseDto putExerciseDto);
    Task RemoveSimpleExercise(User user, Guid id);
    Task<GetSimpleExerciseDto> GetSimpleExercise(User user, Guid id);
    Task<IEnumerable<GetSimpleExerciseDto>> GetSimpleExercises(User user);
}