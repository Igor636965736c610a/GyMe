using GymAppCore.Models.Entities;

namespace GymAppCore.IRepo;

public interface ISimpleExerciseRepo
{
    Task<SimpleExercise?> Get(Guid id);
    Task<List<SimpleExercise>> GetAll(Guid exerciseId, int page, int size);
    Task Create(SimpleExercise exercise);
    Task Update(SimpleExercise exercise);
    Task Remove(SimpleExercise exercise);
}