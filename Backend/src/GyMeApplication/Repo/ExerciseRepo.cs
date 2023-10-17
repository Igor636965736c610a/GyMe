using GyMeApplication.Options;
using GyMeCore.IRepo;
using GyMeCore.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GyMeApplication.Repo;

internal class ExerciseRepo : IExerciseRepo
{
    private readonly GyMePostgresContext _gyMePostgresContext;
    public ExerciseRepo(GyMePostgresContext gyMePostgresContext)
    {
        _gyMePostgresContext = gyMePostgresContext;
    }
    
    public async Task<Exercise?> Get(Guid exerciseId)
        => await _gyMePostgresContext.Exercises.Include(x => x.ConcreteExercise).FirstOrDefaultAsync(x => x.Id == exerciseId);

    public async Task<Exercise?> Get(Guid userId, string exercisesType)
        => await _gyMePostgresContext.Exercises.FirstOrDefaultAsync(x =>
            x.UserId == userId && x.ExercisesType == exercisesType);

    public async Task<IEnumerable<Exercise>> GetAll(IEnumerable<Guid> exerciseIds)
        => await _gyMePostgresContext.Exercises.Where(x => exerciseIds.Contains(x.Id))
            .ToListAsync();

    public async Task<List<Exercise>> GetAll(Guid userId, int page, int size)
        => await _gyMePostgresContext.Exercises.Where(x => x.UserId == userId).OrderBy(x => x.Position)
            .Skip(page * size)
            .Take(size)
            .ToListAsync();

    public async Task<List<Exercise>> GetAll(Guid userId)
        => await _gyMePostgresContext.Exercises.Where(x => x.UserId == userId).OrderBy(x => x.Position).ToListAsync();

    public async Task<List<Exercise>> GetAll(Guid userId, IEnumerable<string> exercisesType)
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
        => await _gyMePostgresContext.SimpleExercises
            .Where(x => x.ExerciseId == exerciseId)
            .OrderBy(e => e.TimeStamp).Take(period)
            .Select(e => calculate(e.Series))
            .ToListAsync();

    public async Task<Dictionary<Guid, IEnumerable<int>>> GetScores(IEnumerable<Guid> exercisesId, int period,
        Func<IEnumerable<Series>, int> calculate)
        => await _gyMePostgresContext.Exercises
            .Where(x => exercisesId.Contains(x.Id))
            .Select(x => new
            {
                Value = x.ConcreteExercise.OrderBy(e => e.TimeStamp).Take(period).Select(e => e.Series),
                Key = x.Id
            })
            .ToDictionaryAsync(x => x.Key, x => x.Value.Select(y => calculate(y)));

    public async Task<Dictionary<string, IEnumerable<int>>> GetScores(IEnumerable<string> exercisesType, Guid userId, int period, Func<IEnumerable<Series>, int> calculate)
        => await _gyMePostgresContext.Exercises
            .Where(x => x.UserId == userId && exercisesType.Contains(x.ExercisesType))
            .Select(x => new
            {
                Value = x.ConcreteExercise.OrderBy(e => e.TimeStamp).Take(period).Select(e => e.Series),
                Key = x.ExercisesType.ToString()
            })
            .ToDictionaryAsync(x => x.Key, x => x.Value.Select(y => calculate(y)));

    public async Task Create(Exercise exercise)
    {
        await _gyMePostgresContext.Exercises.AddAsync(exercise);
        await _gyMePostgresContext.SaveChangesAsync();
    }

    public async Task Update(Exercise exercise)
    {
        await Task.FromResult(_gyMePostgresContext.Update(exercise));
        await _gyMePostgresContext.SaveChangesAsync();
    }

    public async Task Update(List<Exercise> exercises)
    {
        _gyMePostgresContext.UpdateRange(exercises);
        await _gyMePostgresContext.SaveChangesAsync();
    }

    public async Task Remove(Exercise exercise)
    {
        _gyMePostgresContext.Remove(exercise);
        await _gyMePostgresContext.SaveChangesAsync();
    }
}