using GymAppCore.Models.Entities;

namespace GymAppCore.IRepo;

public interface IExerciseRepo
{
    Task<Exercise?> Get(User user, ExercisesType exercisesType);
    Task<IEnumerable<Exercise>> Get(User user);
    Task<bool> Create(Exercise exercise);
    Task<bool> Update(Exercise exercise);
    Task<bool> Hide(Exercise exercise);
}