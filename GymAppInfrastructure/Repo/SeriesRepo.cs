using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Context;

namespace GymAppInfrastructure.Repo;

public class SeriesRepo : ISeriesRepo
{
    private readonly GymAppContext _gymAppContext;
    public SeriesRepo(GymAppContext gymAppContext)
    {
        _gymAppContext = gymAppContext;
    }

    public async Task<bool> Create(List<Series> series)
    {
        await _gymAppContext.Series.AddRangeAsync(series);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Update(List<Series> series)
    {
        _gymAppContext.Series.UpdateRange(series);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Remove(List<Series> series)
    {
        _gymAppContext.Series.RemoveRange(series);
        return await UtilsRepo.Save(_gymAppContext);
    }
}