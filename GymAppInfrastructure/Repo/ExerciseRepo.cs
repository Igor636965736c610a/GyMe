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
    
    public async Task<Dictionary<Guid, Series>> GetMaxReps(IEnumerable<Guid> exercisesId)
        => await _gymAppContext.Series
            .Where(x => exercisesId.Contains(x.SimpleExercise.ExerciseId))
            .GroupBy(x => x.SimpleExercise.ExerciseId)
            .Select(x => new
            {
                Value = x.OrderByDescending(e => e.Weight).ThenByDescending(e => e.NumberOfRepetitions).First(),
                x.Key
            })
            .ToDictionaryAsync(x => x.Key, x => x.Value);
    
    public async Task<Series?> GetMaxRep(Guid exerciseId)
        => await _gymAppContext.Series
            .Where(s => s.SimpleExercise.ExerciseId == exerciseId)
            .OrderByDescending(s => s.Weight)
            .ThenByDescending(s => s.NumberOfRepetitions)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<int>> GetScore(Guid exerciseId, int size,
        Func<IEnumerable<Series>, int> calculate)
        => await _gymAppContext.Series
            .Where(x => x.SimpleExercise.ExerciseId == exerciseId)
            .GroupBy(x => x.SimpleExercise)
            .OrderBy(x => x.Key.Date)
            .Take(size)
            .Select(x => calculate(x))
            .ToListAsync();
    
    public async Task<Dictionary<Guid, IEnumerable<int>>> GetScores(IEnumerable<Guid> exercisesId, int size,
        Func<IEnumerable<Series>, int> calculate)
        => await _gymAppContext.Series
            .Where(x => exercisesId.Contains(x.SimpleExercise.ExerciseId))
            .GroupBy(x => x.SimpleExercise)
            .GroupBy(x => x.Key.ExerciseId)
            .Select(x => new
            {
                Value = x.OrderBy(e => e.Key.Date).Take(size),
                x.Key
            })
            .ToDictionaryAsync(x => x.Key, x => x.Value.Select(y => calculate(y)));

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