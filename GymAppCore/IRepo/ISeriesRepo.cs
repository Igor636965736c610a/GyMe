using GymAppCore.Models.Entities;

namespace GymAppCore.IRepo;

public interface ISeriesRepo
{
    Task<bool> Create(List<Series> series);
    Task<bool> Update(List<Series> series);
    Task<bool> Remove(List<Series> series);
}