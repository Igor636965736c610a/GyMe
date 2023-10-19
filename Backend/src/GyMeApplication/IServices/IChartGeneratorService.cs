using GyMeApplication.Models.Exercise;
using GyMeApplication.Options;

namespace GyMeApplication.IServices;

public interface IChartGeneratorService
{
    Task<IEnumerable<int>?> Get(Guid exerciseId, ChartOption option, int period);
    Task<IEnumerable<int>?> Get(Guid userUd, ExercisesTypeDto exercisesTypeDto, ChartOption option,
        int period);
    Task<Dictionary<Guid, IEnumerable<int>>> Get(Guid userId, IEnumerable<Guid> exercisesIds, ChartOption option, int period);
    Task<Dictionary<string, IEnumerable<int>>> Get(Guid userId, IEnumerable<ExercisesTypeDto> exercisesTypeDto, ChartOption option, 
        int period);
}