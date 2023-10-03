using GymAppInfrastructure.Models.Exercise;
using GymAppInfrastructure.Options;

namespace GymAppInfrastructure.IServices;

public interface IChartService
{
    Task<IEnumerable<int>?> Get(Guid exerciseId, ChartOption option, int period);
    Task<IEnumerable<int>?> Get(Guid userUd, ExercisesTypeDto exercisesTypeDto, ChartOption option,
        int period);
    Task<Dictionary<Guid, IEnumerable<int>>?> Get(Guid userId, IEnumerable<Guid> ids, ChartOption option, int period);
    Task<Dictionary<string, IEnumerable<int>>?> Get(Guid userId, IEnumerable<ExercisesTypeDto> exercisesTypeDto, ChartOption option, 
        int period);
}