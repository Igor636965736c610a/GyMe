using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GymAppInfrastructure.Repo;

internal class SimpleExerciseRepo : ISimpleExerciseRepo
{
    private readonly GymAppContext _gymAppContext;
    public SimpleExerciseRepo(GymAppContext gymAppContext)
    {
        _gymAppContext = gymAppContext;
    }
    
    public async Task<SimpleExercise?> Get(Guid id)
        => await _gymAppContext.SimpleExercises.Include(x => x.Series).FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<SimpleExercise>> GetAll(Guid userId, Guid exerciseId, int page, int size)
        => await _gymAppContext.SimpleExercises.Where(x => x.UserId == userId && x.ExerciseId == exerciseId)
            .OrderBy(x => x.Date)
            .Include(x => x.Series)
            .Skip(page*size)
            .Take(size)
            .ToListAsync();

    public async Task<bool> Create(SimpleExercise exercise)
    {
        await _gymAppContext.SimpleExercises.AddAsync(exercise);
        return await UtilsRepo.SaveDatabaseChanges(_gymAppContext);
    }

    public async Task<bool> Update(SimpleExercise exercise)
    {
        _gymAppContext.SimpleExercises.Update(exercise);
        return await UtilsRepo.SaveDatabaseChanges(_gymAppContext);
    }

    public async Task<bool> Remove(SimpleExercise exercise)
    {
        _gymAppContext.SimpleExercises.Remove(exercise);
        return await UtilsRepo.SaveDatabaseChanges(_gymAppContext);
    }
}