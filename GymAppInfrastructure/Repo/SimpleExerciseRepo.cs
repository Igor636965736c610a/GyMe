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
    
    public async Task<SimpleExercise?> Get(Guid id)
        => await _gymAppContext.SimpleExercises.Include(x => x.Series).FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<SimpleExercise>> GetAll(Guid userId, Guid exerciseId, int page, int size)
        => await _gymAppContext.SimpleExercises.Where(x => x.UserId == userId && x.ExerciseId == exerciseId).Include(x => x.Series)
            .Skip(page*size)
            .Take(size).ToListAsync();

    public async Task<List<SimpleExercise>> GetAll(Guid userId, Guid exerciseId)
        => await _gymAppContext.SimpleExercises.Where(x => x.UserId == userId && x.ExerciseId == exerciseId).Include(x => x.Series).ToListAsync();

    public async Task<Dictionary<Guid, string?>> GetMaxReps(Guid userId, IEnumerable<Guid> exercisesIds)
        => await _gymAppContext.SimpleExercises
            .Where(x => x.UserId == userId && exercisesIds.Contains(x.ExerciseId))
            .Include(x => x.Series)
            .GroupBy(x => x.ExerciseId)
            .ToDictionaryAsync(
                g => g.Key,
                g => g.SelectMany(x => x.Series)
                    .OrderByDescending(x => x.Weight)
                    .ThenByDescending(x => x.NumberOfRepetitions)
                    .Select(x => $"{x.Weight}, {x.NumberOfRepetitions}")
                    .FirstOrDefault()
            );

    public async Task<string?> GetMaxRep(Guid userId, Guid exercisesId)
        => await _gymAppContext.SimpleExercises
            .Where(x => x.UserId == userId && x.ExerciseId == exercisesId)
            .Include(x => x.Series)
            .SelectMany(x => x.Series)
            .OrderByDescending(x => x.Weight)
            .ThenByDescending(x => x.NumberOfRepetitions)
            .Select(x => $"{x.Weight}, {x.NumberOfRepetitions}")
            .FirstOrDefaultAsync();
    
    public async Task<bool> Create(SimpleExercise exercise)
    {
        await _gymAppContext.SimpleExercises.AddAsync(exercise);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Update(SimpleExercise exercise)
    {
        _gymAppContext.SimpleExercises.Update(exercise);
        return await UtilsRepo.Save(_gymAppContext);
    }

    public async Task<bool> Remove(SimpleExercise exercise)
    {
        _gymAppContext.SimpleExercises.Remove(exercise);
        return await UtilsRepo.Save(_gymAppContext);
    }
}