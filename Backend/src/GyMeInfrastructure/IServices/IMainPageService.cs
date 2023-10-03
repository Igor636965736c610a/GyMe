using GyMeInfrastructure.Services;

namespace GyMeInfrastructure.IServices;

public interface IMainPageService
{
    Task<IEnumerable<SimpleExercisePageElement>> GetNewSimpleExercisesForMainPage(int page, int size);
    Task<IEnumerable<SimpleExercisePageElement>> GetPastSimpleExercisesForMainPage(int page, int size);
}