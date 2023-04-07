using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GymAppInfrastructure.Repo;

public class SimpleExerciseRepo : ISimpleExerciseRepo
{
    private readonly GymAppContext _gymAppContext;
    public SimpleExerciseRepo(GymAppContext gymAppContext)
    {
        _gymAppContext = gymAppContext;
    }
    
    public async Task<SimpleExercise?> Get(User user, Guid id)
        => await _gymAppContext.SimpleExercises.FirstOrDefaultAsync(x => x.User == user && x.Id == id);

    public async Task<IEnumerable<SimpleExercise>> Get(User user)
        => await Task.FromResult(_gymAppContext.SimpleExercises.Where(x => x.User == user));

    public async Task<bool> Create(SimpleExercise exercise)
    {
        await _gymAppContext.SimpleExercises.AddAsync(exercise);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Update(SimpleExercise exercise)
    {
        await Task.FromResult(_gymAppContext.SimpleExercises.Update(exercise));
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Remove(SimpleExercise exercise)
    {
        await Task.FromResult(_gymAppContext.SimpleExercises.Remove(exercise));
        return await UtilsRepo.Save(_gymAppContext);
    }
}