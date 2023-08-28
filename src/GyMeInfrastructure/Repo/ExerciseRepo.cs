using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Options;
using Microsoft.EntityFrameworkCore;

namespace GymAppInfrastructure.Repo;

internal class ExerciseRepo : IExerciseRepo
{
    private readonly GyMePostgresContext _gyMePostgresContext;
    public ExerciseRepo(GyMePostgresContext gyMePostgresContext)
    {
        _gyMePostgresContext = gyMePostgresContext;
    }
    
    public async Task<Exercise?> Get(Guid exerciseId)
        => await _gyMePostgresContext.Exercises.FirstOrDefaultAsync(x => x.Id == exerciseId);

    public async Task<Exercise?> Get(Guid userId, ExercisesType exercisesType)
        => await _gyMePostgresContext.Exercises.FirstOrDefaultAsync(x =>
            x.UserId == userId && x.ExercisesType == exercisesType);

    public async Task<IEnumerable<Exercise>> GetAll(IEnumerable<Guid> exerciseIds)
        => await _gyMePostgresContext.Exercises.Where(x => exerciseIds.Contains(x.Id))
            .ToListAsync();

    public async Task<List<Exercise>> GetAll(Guid userId, int page, int size)
        => await _gyMePostgresContext.Exercises.Where(x => x.UserId == userId).OrderBy(x => x.Position)
            .Skip(page * size)
            .Take(size).ToListAsync();

    public async Task<List<Exercise>> GetAll(Guid userId)
        => await _gyMePostgresContext.Exercises.Where(x => x.UserId == userId).OrderBy(x => x.Position).ToListAsync();

    public async Task<List<Exercise>> GetAll(Guid userId, IEnumerable<ExercisesType> exercisesType)
        => await _gyMePostgresContext.Exercises.Where(x => x.UserId == userId && exercisesType.Contains(x.ExercisesType)).OrderBy(x => x.Position).ToListAsync();

    public async Task<Dictionary<Guid, Series>> GetMaxReps(IEnumerable<Guid> exercisesId)
        => await _gyMePostgresContext.Exercises
            .Where(x => exercisesId.Contains(x.Id))
            .Select(x => new
            {
                Value = x.ConcreteExercise.SelectMany(e => e.Series).OrderByDescending(e => e.Weight)
                    .ThenByDescending(e => e.NumberOfRepetitions).First(),
                Key = x.Id
            })
            .ToDictionaryAsync(x => x.Key, x => x.Value);
    
    public async Task<Series?> GetMaxRep(Guid exerciseId)
        => await _gyMePostgresContext.Series
            .Where(s => s.SimpleExercise.ExerciseId == exerciseId)
            .OrderByDescending(s => s.Weight)
            .ThenByDescending(s => s.NumberOfRepetitions)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<int>> GetScore(Guid exerciseId, int period,
        Func<IEnumerable<Series>, int> calculate)
        => await Task.FromResult(_gyMePostgresContext.SimpleExercises
            .Where(x => x.ExerciseId == exerciseId)
            .OrderBy(e => e.Date).Take(period).Select(e => calculate(e.Series)));

    public async Task<Dictionary<Guid, IEnumerable<int>>> GetScores(IEnumerable<Guid> exercisesId, int period,
        Func<IEnumerable<Series>, int> calculate)
        => await _gyMePostgresContext.Exercises
            .Where(x => exercisesId.Contains(x.Id))
            .Select(x => new
            {
                Value = x.ConcreteExercise.OrderBy(e => e.Date).Take(period).Select(e => e.Series),
                Key = x.Id
            })
            .ToDictionaryAsync(x => x.Key, x => x.Value.Select(y => calculate(y)));

    public async Task<Dictionary<string, IEnumerable<int>>> GetScores(IEnumerable<ExercisesType> exercisesType, Guid userId, int period, Func<IEnumerable<Series>, int> calculate)
        => await _gyMePostgresContext.Exercises
            .Where(x => x.UserId == userId && exercisesType.Contains(x.ExercisesType))
            .Select(x => new
            {
                Value = x.ConcreteExercise.OrderBy(e => e.Date).Take(period).Select(e => e.Series),
                Key = x.ExercisesType.ToString()
            })
            .ToDictionaryAsync(x => x.Key, x => x.Value.Select(y => calculate(y)));

    public async Task<bool> Create(Exercise exercise)
    {
        await _gyMePostgresContext.Exercises.AddAsync(exercise);
        return await UtilsRepo.SaveDatabaseChanges(_gyMePostgresContext);
    }

    public async Task<bool> Update(Exercise exercise)
    {
        await Task.FromResult(_gyMePostgresContext.Update(exercise));
        return await UtilsRepo.SaveDatabaseChanges(_gyMePostgresContext);
    }

    public async Task<bool> Update(List<Exercise> exercises)
    {
        _gyMePostgresContext.UpdateRange(exercises);
        return await UtilsRepo.SaveDatabaseChanges(_gyMePostgresContext);
    }

    public async Task<bool> Remove(Exercise exercise)
    {
        _gyMePostgresContext.Remove(exercise);
        return await UtilsRepo.SaveDatabaseChanges(_gyMePostgresContext);
    }
}