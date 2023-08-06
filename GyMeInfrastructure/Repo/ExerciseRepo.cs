using GymAppCore.IRepo;
using GymAppCore.Models.Entities;
using GymAppInfrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GymAppInfrastructure.Repo;

internal class ExerciseRepo : IExerciseRepo
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

    public async Task<IEnumerable<Exercise>> GetAll(IEnumerable<Guid> exerciseIds)
        => await _gymAppContext.Exercises.Where(x => exerciseIds.Contains(x.Id))
            .ToListAsync();

    public async Task<List<Exercise>> GetAll(Guid userId, int page, int size)
        => await _gymAppContext.Exercises.Where(x => x.UserId == userId).OrderBy(x => x.Position)
            .Skip(page * size)
            .Take(size).ToListAsync();

    public async Task<List<Exercise>> GetAll(Guid userId)
        => await _gymAppContext.Exercises.Where(x => x.UserId == userId).OrderBy(x => x.Position).ToListAsync();

    public async Task<List<Exercise>> GetAll(Guid userId, IEnumerable<ExercisesType> exercisesType)
        => await _gymAppContext.Exercises.Where(x => x.UserId == userId && exercisesType.Contains(x.ExercisesType)).OrderBy(x => x.Position).ToListAsync();

    public async Task<Dictionary<Guid, Series>> GetMaxReps(IEnumerable<Guid> exercisesId)
        => await _gymAppContext.Exercises
            .Where(x => exercisesId.Contains(x.Id))
            .Select(x => new
            {
                Value = x.ConcreteExercise.SelectMany(e => e.Series).OrderByDescending(e => e.Weight)
                    .ThenByDescending(e => e.NumberOfRepetitions).First(),
                Key = x.Id
            })
            .ToDictionaryAsync(x => x.Key, x => x.Value);
    
    public async Task<Series?> GetMaxRep(Guid exerciseId)
        => await _gymAppContext.Series
            .Where(s => s.SimpleExercise.ExerciseId == exerciseId)
            .OrderByDescending(s => s.Weight)
            .ThenByDescending(s => s.NumberOfRepetitions)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<int>?> GetScore(Guid exerciseId, int period,
        Func<IEnumerable<Series>, int> calculate)
        => await _gymAppContext.Exercises
            .Where(x => x.Id == exerciseId)
            .Select(x => x.ConcreteExercise.OrderBy(e => e.Date).Take(period).Select(e => calculate(e.Series)))
            .FirstOrDefaultAsync();

    public async Task<Dictionary<Guid, IEnumerable<int>>?> GetScores(IEnumerable<Guid> exercisesId, int period,
        Func<IEnumerable<Series>, int> calculate)
        => await _gymAppContext.Exercises
            .Where(x => exercisesId.Contains(x.Id))
            .Select(x => new
            {
                Value = x.ConcreteExercise.OrderBy(e => e.Date).Take(period).Select(e => e.Series),
                Key = x.Id
            })
            .ToDictionaryAsync(x => x.Key, x => x.Value.Select(y => calculate(y)));

    public async Task<Dictionary<string, IEnumerable<int>>?> GetScores(IEnumerable<ExercisesType> exercisesType, Guid userId, int period, Func<IEnumerable<Series>, int> calculate)
        => await _gymAppContext.Exercises
            .Where(x => x.UserId == userId && exercisesType.Contains(x.ExercisesType))
            .Select(x => new
            {
                Value = x.ConcreteExercise.OrderBy(e => e.Date).Take(period).Select(e => e.Series),
                Key = x.ExercisesType.ToString()
            })
            .ToDictionaryAsync(x => x.Key, x => x.Value.Select(y => calculate(y)));

        public async Task<bool> Create(Exercise exercise)
    {
        await _gymAppContext.Exercises.AddAsync(exercise);
        return await UtilsRepo.SaveDatabaseChanges(_gymAppContext);
    }

    public async Task<bool> Update(Exercise exercise)
    {
        await Task.FromResult(_gymAppContext.Update(exercise));
        return await UtilsRepo.SaveDatabaseChanges(_gymAppContext);
    }

    public async Task<bool> Update(List<Exercise> exercises)
    {
        _gymAppContext.UpdateRange(exercises);
        return await UtilsRepo.SaveDatabaseChanges(_gymAppContext);
    }

    public async Task<bool> Remove(Exercise exercise)
    {
        _gymAppContext.Remove(exercise);
        return await UtilsRepo.SaveDatabaseChanges(_gymAppContext);
    }
}