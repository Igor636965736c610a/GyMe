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
    
    public async Task<Exercise?> Get(Guid exerciseId)
        => await _gymAppContext.Exercises.FirstOrDefaultAsync(x => x.Id == exerciseId);

    public async Task<Exercise?> Get(Guid userId, ExercisesType exercisesType)
        => await _gymAppContext.Exercises.FirstOrDefaultAsync(x =>
            x.UserId == userId && x.ExercisesType == exercisesType);

    public async Task<List<Exercise>> GetAll(Guid userId, int page, int size)
        => await _gymAppContext.Exercises.Where(x => x.UserId == userId).OrderBy(x => x.Position)
            .Skip(page * size)
            .Take(size).ToListAsync();

    public async Task<List<Exercise>> GetAll(Guid userId)
        => await _gymAppContext.Exercises.Where(x => x.UserId == userId).OrderBy(x => x.Position).ToListAsync();
    
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