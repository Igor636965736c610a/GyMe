using GymAppInfrastructure.Options;

namespace GymAppInfrastructure.IServices;

public interface IChartService
{
    Task<IEnumerable<int>?> Get(Guid jwtId, Guid exerciseId, ChartOption option, int period);
    Task<Dictionary<Guid, IEnumerable<int>>?> Get(Guid jwtId, Guid userId, IEnumerable<Guid> ids, ChartOption option, int period);
}