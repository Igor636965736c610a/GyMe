using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Options;
using Microsoft.EntityFrameworkCore;

namespace GymAppInfrastructure.Repo;

internal class SimpleExerciseRepo : ISimpleExerciseRepo
{
    private readonly GyMePostgresContext _gyMePostgresContext;
    public SimpleExerciseRepo(GyMePostgresContext gyMePostgresContext)
    {
        _gyMePostgresContext = gyMePostgresContext;
    }
    
    public async Task<SimpleExercise?> Get(Guid id)
        => await _gyMePostgresContext.SimpleExercises.Include(x => x.Series).FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<SimpleExercise>> GetAll(Guid userId, Guid exerciseId, int page, int size)
        => await _gyMePostgresContext.SimpleExercises.Where(x => x.UserId == userId && x.ExerciseId == exerciseId)
            .OrderBy(x => x.Date)
            .Include(x => x.Series)
            .Skip(page*size)
            .Take(size)
            .ToListAsync();

    public async Task<bool> Create(SimpleExercise exercise)
    {
        await _gyMePostgresContext.SimpleExercises.AddAsync(exercise);
        return await UtilsRepo.SaveDatabaseChanges(_gyMePostgresContext);
    }

    public async Task<bool> Update(SimpleExercise exercise)
    {
        _gyMePostgresContext.SimpleExercises.Update(exercise);
        return await UtilsRepo.SaveDatabaseChanges(_gyMePostgresContext);
    }

    public async Task<bool> Remove(SimpleExercise exercise)
    {
        _gyMePostgresContext.SimpleExercises.Remove(exercise);
        return await UtilsRepo.SaveDatabaseChanges(_gyMePostgresContext);
    }
}