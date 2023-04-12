using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GymAppInfrastructure.Repo;

public class ExerciseRepo : IExerciseRepo
{
    private readonly GymAppContext _gymAppContext;
    public ExerciseRepo(GymAppContext gymAppContext)
    {
        _gymAppContext = gymAppContext;
    }
    
    public async Task<Exercise?> Get(ExercisesType exercisesType, Guid userId)
        => await _gymAppContext.Exercises.FirstOrDefaultAsync(x => x.UserId == userId && x.ExercisesType == exercisesType);

    public async Task<List<Exercise>> Get(Guid userId)
        => await _gymAppContext.Exercises.Where(x => x.UserId == userId).OrderBy(x => x.Position).Include(x => x.ConcreteExercise).ToListAsync();

    public async Task<bool> Create(Exercise exercise)
    {
        await _gymAppContext.Exercises.AddAsync(exercise);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Update(Exercise exercise)
    {
        await Task.FromResult(_gymAppContext.Update(exercise));
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Update(List<Exercise> exercises)
    {
        _gymAppContext.UpdateRange(exercises);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Remove(Exercise exercise)
    {
        _gymAppContext.Remove(exercise);
        return await UtilsRepo.Save(_gymAppContext);
    }
}