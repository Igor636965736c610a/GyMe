using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Exercise;

namespace GymAppInfrastructure.IServices;

public interface IExerciseService
{
    Task CreateExercise(PostExerciseDto postExerciseDto, Guid userId);
    Task UpdateExercise(Guid userId, ExercisesType exercisesType, PutExerciseDto putExerciseDto);
    Task RemoveExercise(Guid userId, ExercisesType exercisesType);
    Task<GetExerciseDto> GetExercise(Guid userId, ExercisesType exercisesType);
    Task<IEnumerable<GetExerciseDto>> GetExercises(Guid userId);
}