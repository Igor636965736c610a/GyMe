using GymAppCore.Models.Entities;

namespace GymAppCore.IRepo;

public interface ISimpleExerciseRepo
{
    Task<SimpleExercise?> Get(User user, Guid id);
    Task<IEnumerable<SimpleExercise>> Get(User user);
    Task<bool> Create(SimpleExercise exercise);
    Task<bool> Update(SimpleExercise exercise);
    Task<bool> Remove(SimpleExercise exercise);
}