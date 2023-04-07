using GymAppCore.Models.Entities;
using GymAppInfrastructure.Dtos.Exercise;

namespace GymAppInfrastructure.IServices;

public interface IExerciseService
{
    Task CreateExercise(PostExerciseDto postExerciseDto, User user);
    Task UpdateExercise(User user, ExercisesType exercisesType, PutExerciseDto putExerciseDto);
    Task HideExercise(User user, ExercisesType exercisesType);
    Task<GetExerciseDto> GetExercise(User user, ExercisesType exercisesType);
    Task<IEnumerable<GetExerciseDto>> GetExercises(User user);
}